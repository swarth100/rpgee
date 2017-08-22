using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerIOClient;
using EEPhysics;

namespace RPGEE
{
    class BackgroundThread
    {
        public enum Actions
        {
            None,
            InitParse,
        }

        /* Class used when signalling the computational thread with actions */
        public class ActionEvent
        {
            public Actions action { get; set; }
            public Object data { get; set; }

            public ActionEvent(Actions action, Object data)
            {
                this.action = action;
                this.data = data;
            }
        }


        /* Public objects held by BackgroundThreads */
        public static PhysicsWorld Physicsworld = new PhysicsWorld();
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();

        /* Thread communication object */
        public static Queue<ActionEvent> activityQueue = new Queue<ActionEvent>();
        public static Object _activityLock = new Object();

        /* RoomData object */
        public static uint[,,] roomData;
        public static Object _roomDataLock = new Object();
        public static int width = 0;
        public static int height = 0;

        /* Class fields */
        private static string email = "";
        private static string password = "";
        private static string roomid = "";

        #region ComputationalThread

        /** Computational thread main function.
         * The computational thread is meant to perform a number of heavy weight operations leaving both the UI and
         * the connections thread free to handle their other tasks.
         * All computational events must be packaged into an ActionEvent and added to the activityQueue.
         * The computational thread keeps listening on the queue, popping out ActionEvents as they show up. */
        public static void runComputationsThread()
        {
            ActionEvent activity;

            /* Main computational thread loop */
            while (true)
            {
                /* Check for new ActionEvents, if any present Dequeue them */
                lock (_activityLock)
                {
                    if (activityQueue.Count != 0)
                    {
                        activity = activityQueue.Dequeue();
                    }
                    else
                    {
                        activity = null;
                    }
                }

                /* When a new activity is detected, detect the relevant action */
                if (activity != null)
                {
                    switch (activity.action)
                    {
                        case Actions.None:
                            break;
                        case Actions.InitParse:
                            /** InitParse is the action meant to parse the world data dumped by an "init" even when joining a world.
                             * Successful InitParse completion will trigger the rendering of the main Map image. */

                            /* Cast data back to message type */
                            Message e = (Message)activity.data;

                            /** Initialise and populate roomData 3D array
                             * Credits to:
                             * https://gist.github.com/Yonom/3c9ebfe69b1432452f9b */
                            lock (_roomDataLock)
                            {
                                roomData = new uint[2, e.GetInt(18), e.GetInt(19)];

                                /* Initialise width end height fields */
                                width = e.GetInt(18);
                                height = e.GetInt(19);

                                /* Parse the room's DataCHunks */
                                DataChunk[] chunks = InitParse.Parse(e);
                                foreach (var chunk in chunks)
                                    foreach (var pos in chunk.Locations)
                                        roomData[chunk.Layer, pos.X, pos.Y] = chunk.Type;
                            }

                            /* Force initial map async rendering */
                            RpgEE.map.loadMap();

                            break;
                    }
                }
            }
        }

        #endregion

        #region ConnectionThread

        /** Connection thread main function.
         * The connection thread is means to handle all msg events sent by the PlayerIO server.
         * It also issues all messages that must be sent by the client.
         * It is a priority not to slow down this thread in order to not lose packets. */
        public static void runConnectionThread()
        {
            lock (ConnectionDetails._lock)
            {
                email = ConnectionDetails.username;
                password = ConnectionDetails.password;
                roomid = ConnectionDetails.roomID;
            }

            /** Most of the following code was kindly uploaded by CAPASHA on:
             * https://pastebin.com/maiaKcRD */

            #region playerIOConnection

            /* Connect to the EverybodyEdits server */
            PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw", email, password, null,
            delegate (Client client)
            {
                Console.WriteLine("Logged in!");
                /* Upon successful connection, attempt to join the designated room */
                client.Multiplayer.CreateJoinRoom(roomid, "Everybodyedits" + client.BigDB.Load("config", "config")["version"], true, null, null,
                delegate (Connection con)
                {
                    /* Send the "init" message to join the room and subscribe to the room's messages */
                    con.Send("init");
                    con.OnMessage += delegate (object sender, PlayerIOClient.Message m)
                    {
                        /* EEPhysics library handler initialisation. It pre-parses all messages sent by the server to the client */
                        Physicsworld.HandleMessage(m);
                        switch (m.Type)
                        {
                            case "init":
                                /** "init" messages are sent upon a successful room join.
                                 * They are sent along with the entire room data. */

                                Players.Add(m.GetInt(5), new Player(m.GetInt(5), m.GetString(13)));

                                /* Send an "init2" message to finalise the join */
                                con.Send("init2");

                                /* Delegate initParse event to computationsThread */
                                lock (_activityLock)
                                {
                                    activityQueue.Enqueue(new ActionEvent(Actions.InitParse, m));
                                }

                                break;
                            case "init2":
                                /** Once the "init2" message is received, our bot is ready to be fully operational within the room */

                                /* Force UI update to render home screen */
                                RpgEE.showScreen(RpgEE.Layers.Home);

                                break;
                            case "add":
                                /** "add" messages are sent by players joining the level. */
                                if (!Players.ContainsKey(m.GetInt(0)))
                                {
                                    Players.Add(m.GetInt(0), new Player(m.GetInt(0), m.GetString(1)));
                                    Players[m.GetInt(0)].Physics.OnHitGodBlock += Physics_OnHitGodBlock;
                                    Players[m.GetInt(0)].Physics.OnHitGravityEffect += Physics_OnHitGravityEffect;
                                    Players[m.GetInt(0)].Physics.OnBlockPositionChange += Physics_OnBlockPositionChange;
                                }
                                break;
                            case "left":
                                Players.Remove(m.GetInt(0));
                                break;
                            case "m":
                                /* Movement */
                                break;
                            case "say":
                                /* Messages */
                                Console.WriteLine(m);
                                break;
                        }
                    };
                },
                delegate (PlayerIOError error)
                {
                    /* Handle invalid roomID */
                    Console.WriteLine("Timeout when joining room. Possibly invalid roomID.");
                });

            },
            delegate (PlayerIOError error)
            {
                /* Handle invalidCredentials */
                Console.WriteLine("Invalid credentials");
            });

            #endregion
        }

        #endregion

        #region EEPhysics

        private static void Physics_OnBlockPositionChange(PlayerBlockEventArgs e)
        {
            if (EEPhysics.ItemId.IsSolid(e.BlockId) && !EEPhysics.ItemId.CanJumpThroughFromBelow(e.BlockId) && !e.Player.InGodMode)
            {
                Console.WriteLine(e.Player.Name + " Tried to walk through a block");
            }
        }

        private static void Physics_OnHitGravityEffect(PlayerEventArgs e)
        {
            Console.WriteLine(e.Player.Name + " Hit gravity effect block");
        }

        private static void Physics_OnHitGodBlock(PlayerEventArgs e)
        {
            Console.WriteLine(e.Player.Name + " Hit god block");
        }

        #endregion

    }
}

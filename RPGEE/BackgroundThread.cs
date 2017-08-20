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

        public static void runComputationsThread()
        {
            ActionEvent activity;
            while (true)
            {
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

                if (activity != null)
                {
                    switch (activity.action)
                    {
                        case Actions.None:
                            break;
                        case Actions.InitParse:
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

                            /*
                            Console.WriteLine(roomData[0, 0, 0]);
                            Console.WriteLine(roomData[1, 0, 0]);
                            Console.WriteLine(roomData[0, 1, 1]);
                            */

                            /* Force initial map async rendering */
                            Map.loadMap(RpgEE.mapPct);

                            break;
                    }
                }
            }
        }

        /** Most of the following code was kindly uploaded by CAPASHA on:
         * https://pastebin.com/maiaKcRD */

        public static void runConnectionThread()
        {
            lock (ConnectionDetails._lock)
            {
                email = ConnectionDetails.username;
                password = ConnectionDetails.password;
                roomid = ConnectionDetails.roomID;
            }
            PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw", email, password, null,
            delegate (Client client)
            {
                Console.WriteLine("Logged in!");
                client.Multiplayer.CreateJoinRoom(roomid, "Everybodyedits" + client.BigDB.Load("config", "config")["version"], true, null, null,
                delegate (Connection con)
                {
                    con.Send("init");
                    con.OnMessage += delegate (object sender, PlayerIOClient.Message m)
                    {
                        Physicsworld.HandleMessage(m);
                        switch (m.Type)
                        {
                            case "init":
                                Players.Add(m.GetInt(5), new Player(m.GetInt(5), m.GetString(13)));
                                con.Send("init2");

                                /* Add initParse event to computationsThread */
                                lock (_activityLock)
                                {
                                    activityQueue.Enqueue(new ActionEvent(Actions.InitParse, m));
                                }

                                break;
                            case "init2":
                                /* Force UI update to render home screen */
                                RpgEE.showScreen(RpgEE.Layers.Home);
                                break;
                            case "add":
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

                });

            },
            delegate (PlayerIOError error)
            {

            });
            // Console.ReadKey();
        }

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

    }
}

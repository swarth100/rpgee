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
        /* Public objects held by BackgroundThreads */
        public static PhysicsWorld Physicsworld = new PhysicsWorld();
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();

        /* Class fields */
        private static string email = "";
        private static string password = "";
        private static string roomid = "";

        /** Most of the following code was kindly uploaded by CAPASHA on:
         * https://pastebin.com/maiaKcRD */

        public static void runConnectionThread()
        {
            lock (ConnectionDetails._lock)
            {
                email = ConnectionDetails.username;
                password = ConnectionDetails.password;
                roomid = ConnectionDetails.roomID;

                /* Debug */
                Console.WriteLine(ConnectionDetails.username);
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
                                break;
                            case "init2":
                                Console.WriteLine("Connected to the room.");
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

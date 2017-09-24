using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlayerIOClient;
using EEPhysics;

namespace RPGEE
{
    /** Most of the following code was kindly uploaded by CAPASHA on:
         * https://pastebin.com/maiaKcRD */

    public class Player
    {
        public PhysicsPlayer Physics { get { return BackgroundThread.Physicsworld.Players[Id]; } }

        public string Name { get; }
        public int Id { get; }

        public double X { get { return Physics.X; } }
        public double Y { get { return Physics.Y; } }
        public int x { get { return (int)Math.Round(X / 16); } }
        public int y { get { return (int)Math.Round(X / 16); } }

        public Player(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

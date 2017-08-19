﻿/* 
The MIT License (MIT)
Copyright (c) 2015 Sepehr Farshid
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using PlayerIOClient;
using System.Drawing;

namespace RPGEE
{
    public static class InitParse
    {
        public static DataChunk[] Parse(Message m)
        {
            if (m == null) throw new ArgumentNullException("m");
            if (m.Type != "init" && m.Type != "reset") throw new ArgumentException("Invalid message type.", "m");

            // Get world data
            var p = 0u;
            var data = new Stack<object>();
            while (m[p++] as string != "ws") { }
            while (m[p] as string != "we") { data.Push(m[p++]); }

            // Parse world data
            var chunks = new List<DataChunk>();
            while (data.Count > 0)
            {
                var args = new Stack<object>();
                while (!(data.Peek() is byte[]))
                    args.Push(data.Pop());

                var ys = (byte[])data.Pop();
                var xs = (byte[])data.Pop();
                var layer = (int)data.Pop();
                var type = (uint)data.Pop();

                chunks.Add(new DataChunk(layer, type, xs, ys, args.ToArray()));
            }

            return chunks.ToArray();
        }
    }

    public class DataChunk
    {
        public int Layer { get; set; }
        public uint Type { get; set; }
        public Point[] Locations { get; set; }
        public object[] Args { get; set; }

        public DataChunk(int layer, uint type, byte[] xs, byte[] ys, object[] args)
        {
            this.Layer = layer;
            this.Type = type;
            this.Args = args;
            this.Locations = GetLocations(xs, ys);
        }

        private static Point[] GetLocations(byte[] xs, byte[] ys)
        {
            var points = new List<Point>();
            for (var i = 0; i < xs.Length; i += 2)
                points.Add(new Point(
                    (xs[i] << 8) | xs[i + 1],
                    (ys[i] << 8) | ys[i + 1]));
            return points.ToArray();
        }
    }

    // TODO: Uncomment this if using Console Application
    //public struct Point
    //{
    //    public int X { get; set; }
    //    public int Y { get; set; }

    //    public Point(int x, int y) : this()
    //    {
    //        this.X = x;
    //        this.Y = y;
    //    }
    //}
}

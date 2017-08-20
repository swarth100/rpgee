﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    class DraggablePictureBox : PictureBox
    {
        private bool Dragging;
        private bool Drawing;
        private int newTopPos;
        private int newLeftPos;
        private int maxRightPos;
        private int maxBottomPos;
        private readonly DragMousePosition mousePos;
        private readonly Map map;

        private class DragMousePosition
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public DraggablePictureBox(Map map) : base()
        {
            this.map = map;
            mousePos = new DragMousePosition();

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
        }

        /* Handles release of the mouse button */
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Dragging = false;
            Drawing = false;
        }

        /* Handles mouse button clicking */
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (map.status == Map.Status.Move)
                {
                    Dragging = true;
                    mousePos.X = e.X;
                    mousePos.Y = e.Y;

                    maxBottomPos = RpgEE.getMapHeight();
                    maxRightPos = RpgEE.getMapWidth();
                }
                else if (map.status == Map.Status.Draw)
                {
                    Drawing = true;
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
            {
                if (Dragging)
                {
                    newTopPos = e.Y + c.Top - mousePos.Y;
                    if ((newTopPos < 0) && (newTopPos + c.Height > maxBottomPos))
                        c.Top = newTopPos;

                    newLeftPos = e.X + c.Left - mousePos.X;
                    if ((newLeftPos < 0) && (newLeftPos + c.Width > maxRightPos))
                        c.Left = newLeftPos;
                }
                else if (Drawing)
                {
                    RpgEE.map.drawPoint(new Point(e.X, e.Y));
                }
            }
        }
    }
}

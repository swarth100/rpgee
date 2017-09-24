﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    public class DraggablePictureBox : PictureBox
    {
        private bool Dragging;
        private bool Drawing;
        private bool Erasing;
        public bool Inspecting { get; set; }
        private int newTopPos;
        private int newLeftPos;
        private int maxRightPos;
        private int maxBottomPos;
        private readonly DragMousePosition mousePos;
        private readonly DragMousePosition mouseDrag;
        private readonly Map map;

        private readonly ToolTip inspectTt;

        /** Helper class to keep track of a mouse's position */
        private class DragMousePosition
        {
            public int X { get; set; }
            public int Y { get; set; }

            public void updatePosition(MouseEventArgs e)
            {
                X = e.X;
                Y = e.Y;
            }

            public void resetPositions()
            {
                X = Y = 0;
            }
        }

        public DraggablePictureBox(Map map) : base()
        {
            this.map = map;

            inspectTt = new ToolTip();
            mousePos = new DragMousePosition();
            mouseDrag = new DragMousePosition();

            this.MouseDown += this.OnMouseDown;
            this.MouseMove += this.OnMouseMove;
            this.MouseUp += this.OnMouseUp;
        }

        /* Handles release of the mouse button */
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Dragging = false;
            Drawing = false;
            Erasing = false;
        }

        /* Handles mouse button clicking */
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                /* Set booleans accordingly to the map Status */
                if (map.status == Map.Status.Move)
                {
                    Dragging = true;

                    mousePos.updatePosition(e);
                    mouseDrag.updatePosition(e);

                    /* Prevent map to be dragged out of bounds w.r.t. the container */
                    maxBottomPos = RpgEE.map.PictureBox.Parent.Height;
                    maxRightPos = RpgEE.map.PictureBox.Parent.Width;
                }
                else if (map.status == Map.Status.Draw)
                {
                    Drawing = true;
                }
                else if (map.status == Map.Status.Delete)
                {
                    Erasing = true;
                }

                /* Forcefully reset the saved mouse position and apply a Move Event */
                mouseDrag.resetPositions();
                OnMouseMove(sender, e);
            }
        }

        /** Method to handle mouse movement */
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (c != null && (mouseDrag.X != e.X || mouseDrag.Y != e.Y))
            {
                /* Checks that the given movement occurs during one of the handled drag events */
                if (Dragging)
                {
                    /* This event will move around the DraggableImage which is the map */
                    newTopPos = e.Y + c.Top - mousePos.Y;
                    if ((newTopPos < 0) && (newTopPos + c.Height > maxBottomPos))
                        c.Top = newTopPos;

                    newLeftPos = e.X + c.Left - mousePos.X;
                    if ((newLeftPos < 0) && (newLeftPos + c.Width > maxRightPos))
                        c.Left = newLeftPos;
                }
                else if (Drawing)
                {
                    /* This event will draw new Zone's points onto the map */
                    this.map.drawPoint(new Point(e.X, e.Y));
                }
                else if (Erasing)
                {
                    /* This event will erase a Zone's point from the map */
                    this.map.erasePoint(new Point(e.X, e.Y));
                }
                else if (Inspecting)
                {
                    /* This event inspects the tiles present on the map */
                    this.map.showTooltip(new Point(e.X, e.Y));
                }

                /* Update the saved position of the dragged mouse */
                mouseDrag.updatePosition(e);
            }
        }

        /** Method to handle resize events */
        public void OnResize(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                /* Get the padding that should be applied to center allign the height of the map
                 * with the height of the container */
                int deltaHeight = (this.Parent.Height - this.Height) / 2;

                /* Apply the padding should the height of the container be greater than the map's height */
                if (deltaHeight > 0)
                    this.Top = deltaHeight;
                else if (this.Bounds.Y + this.Height < this.Parent.Height)
                    this.Top = this.Parent.Height - this.Height;

                /* Get the padding that should be applied to center allign the width of the map
                 * with the width of the container */
                int deltaWidth = (this.Parent.Width - this.Width) / 2;

                /* Apply the padding should the width of the container be greater than the map's width */
                if (deltaWidth > 0)
                    this.Left = deltaWidth;
                else if (this.Bounds.X + this.Width < this.Parent.Width)
                    this.Left = this.Parent.Width - this.Width;

                /* Should the container be resized to something small, make sure that the map is anchored
                 * to the top left position */
                if (this.Top > 0 && (this.Parent.Height < this.Height))
                    this.Top = 0;
                if (this.Left > 0 && (this.Parent.Width < this.Width))
                    this.Left = 0;
            }

        }
    }
}

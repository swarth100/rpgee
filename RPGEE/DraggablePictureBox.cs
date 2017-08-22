using System;
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

        private class DragMousePosition
        {
            public int X { get; set; }
            public int Y { get; set; }

            public void updatePosition(MouseEventArgs e)
            {
                X = e.X;
                Y = e.Y;
            }
        }

        public DraggablePictureBox(Map map) : base()
        {
            this.map = map;

            inspectTt = new ToolTip();
            mousePos = new DragMousePosition();
            mouseDrag = new DragMousePosition();

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
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
                if (map.status == Map.Status.Move)
                {
                    Dragging = true;

                    mousePos.updatePosition(e);
                    mouseDrag.updatePosition(e);

                    maxBottomPos = RpgEE.getMapHeight();
                    maxRightPos = RpgEE.getMapWidth();
                }
                else if (map.status == Map.Status.Draw)
                {
                    Drawing = true;
                }
                else if (map.status == Map.Status.Delete)
                {
                    Erasing = true;
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (c != null && (mouseDrag.X != e.X || mouseDrag.Y != e.Y))
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
                else if (Erasing)
                {
                    RpgEE.map.erasePoint(new Point(e.X, e.Y));
                }
                else if (Inspecting)
                {
                    RpgEE.map.showTooltip(new Point(e.X, e.Y));
                }

                mouseDrag.updatePosition(e);
            }
        }
    }
}

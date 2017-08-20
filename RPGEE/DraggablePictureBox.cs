using System;
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

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Dragging = false;
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && map.status == Map.Status.Move)
            {
                Dragging = true;
                mousePos.X = e.X;
                mousePos.Y = e.Y;

                maxBottomPos = RpgEE.getMapHeight();
                maxRightPos = RpgEE.getMapWidth();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (Dragging && c != null)
            {
                newTopPos = e.Y + c.Top - mousePos.Y;
                if ((newTopPos < 0) && (newTopPos + c.Height > maxBottomPos))
                    c.Top = newTopPos;
                
                newLeftPos = e.X + c.Left - mousePos.X;
                if ((newLeftPos < 0) && (newLeftPos + c.Width > maxRightPos))
                    c.Left = newLeftPos;
            }
        }
    }
}

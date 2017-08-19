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
        private readonly DragMousePosition mousePos;

        private class DragMousePosition
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public DraggablePictureBox() : base()
        {
            Console.WriteLine("Called constructor");

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
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                mousePos.X = e.X;
                mousePos.Y = e.Y;
                this.Width = 5000;
                this.Height = 5000;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (Dragging && c != null)
            {
                c.Top = e.Y + c.Top - mousePos.Y;
                c.Left = e.X + c.Left - mousePos.X;
            }
        }
    }
}

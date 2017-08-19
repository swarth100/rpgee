using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    class Map
    {
        public static void loadMap (PictureBox img)
        {
            using (var g = Graphics.FromImage(img.Image))
            {
                //g.DrawEllipse(Pens.Blue, 10, 10, 100, 100);

                // Create image.
                Image newImage = Properties.Resources.BLOCKS_front;

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {

                        // Create rectangle for source image.
                        Rectangle srcRect = new Rectangle((i*20 + j)*16, 0, 16, 16);
                        GraphicsUnit units = GraphicsUnit.Pixel;

                        // Create rectangle for displaying image.
                        Rectangle destRect = new Rectangle(i*16, j*16, 16, 16);

                        // Draw image to screen.
                        g.DrawImage(newImage, destRect, srcRect, units);
                    }
                }

                img.Refresh();
            }
        }
    }
}

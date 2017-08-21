using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    public class Zone
    {
        /* Private fields */
        public Image Image { get; }
        public Brush Brush { get; }
        public String Name { get; set; }
        private int ID { get; }

        private readonly List<Zone> zones;

        private int[,] Data;

        /* Default fields */
        private int opacity = 150;

        public Zone(Image map, List<Zone> list)
        {
            this.Image = new Bitmap(map.Width, map.Height);
            this.Brush = new SolidBrush(getRandomColor());
            this.ID = RpgEE.ZoneID++;

            this.Name = "Zone" + this.ID;

            /* Store a reference to the Zone's list */
            this.zones = list;

            /* Create the 2D array to store the data held by the overlay Zone */
            this.Data = new int[map.Width/Map.blockSize + 1, map.Height/Map.blockSize + 1];

            /* */
            Generator<Form>.addMapListItem(RpgEE.sideNavListView, this);

            /* Append the new element to the list of zones */
            list.Add(this);
        }

        /** Public function to return a given Zone's list index based upon it's ID */
        public int getListIndex()
        {
            return zones.FindIndex(FindByID(this.ID));
        }

        /** Public method to set a specific Point's Zone data to true */
        public void addPoint (Point pt)
        {
            setDataHelper(pt, 1);
        }

        /** Public method to verify if a specific point's data has already been set */
        public bool isPointSelected (Point pt)
        {
            return Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] == 1;
        }

        /** Public method to un-set a specific Point's Zone data, reverting it back to false */
        public void removePoint(Point pt)
        {
            setDataHelper(pt, 0);
        }

        /** Public method to select a given Zone
         * It turns the background behind the Label blue (when selected) */
        public void selectBackground()
        {
            Label name = (Label) RpgEE.sideNavListView.GetEmbeddedControl(0, this.getListIndex());
            name.BackColor = Color.CornflowerBlue;
        }

        /** Public method to de-select a given Zone
         * It turns the background behind the Label back to white */
        public void unselectBackground()
        {
            Label name = (Label)RpgEE.sideNavListView.GetEmbeddedControl(0, this.getListIndex());
            name.BackColor = Color.Transparent;
        }

        /** Public method to change the Zone's Brush's color
         * It updates also the BackColor of the Zone's color selector button */
        public void changeColor(Color newColor)
        {
            /* Must change the fully transparent newColor into a new color which takes account of opacity */
            Color transparentColor = Color.FromArgb(opacity, newColor.R, newColor.G, newColor.B);

            /* Set the Brush's new color */
            (Brush as SolidBrush).Color = transparentColor;

            /* Determine the Zone's corresponding color Button and change the background */
            Button colorBtn = (Button)RpgEE.sideNavListView.GetEmbeddedControl(2, this.getListIndex());
            colorBtn.BackColor = transparentColor;
        }

        /** Private helper method to handle Zone data selection/deselection */
        private void setDataHelper (Point pt, int x)
        {
            Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] = x;
        }

        /** Private helper method to sort out Zones by ID */
        private static Predicate<Zone> FindByID (int ID)
        {
            return delegate (Zone zone)
            {
                return zone.ID == ID;
            };
        }

        /** Private color helper randomiser */
        private Color getRandomColor()
        {
            /* If opacity is != from 255 it is semi-transparent */
            return Color.FromArgb(opacity, RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256));
        }
    }
}

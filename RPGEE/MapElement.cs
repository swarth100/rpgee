using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    /** Public MapElement class is a parent for Map Zones and Map Locations
     * MapElements have Images and Brushes associated to them.
     * They have a name and a Type
     * They are collected into a list from which they can be ordered and/or removed */
    public class MapElement
    {
        public Image Image { get; set; }
        public Brush Brush { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public bool Visible { get; set; }

        /* Private fields */
        protected int ID { get; }

        private readonly List<MapElement> ElementList;

        public MapElement(Image map, List<MapElement> list)
        {
            this.ID = RpgEE.ZoneID++;
            this.Visible = true;

            /* Store a reference to the Object's reference list */
            this.ElementList = list;

            /* Append the new element to the list of zones */
            list.Add(this);
        }

        /** Public function to return a given Zone's list index based upon it's ID */
        public int getListIndex()
        {
            return ElementList.FindIndex(FindByID(this.ID));
        }

        /** Public function to remove the current Map Element from the list of available elements */
        public void removeListElement()
        {
            int baseIndex = this.getListIndex();
            this.Visible = false;

            RpgEE.map.resetSelectedZone(baseIndex);

            RpgEE.sideNavListView.Items[baseIndex].Remove();

            /* Iterates through the whole list of elements in the Extended ListView
             * Every element AFTER the removed one must have its contents shifted down by a row,
             * or otherwise they can no longer be accessed via index [col, row] */
            for (int i = baseIndex; i <= RpgEE.sideNavListView.Items.Count; i++)
            {
                for (int j = 0; j < RpgEE.sideNavListView.Columns.Count; j++)
                {
                    Control embeddedControl = RpgEE.sideNavListView.GetEmbeddedControl(j, i);
                    RpgEE.sideNavListView.RemoveEmbeddedControl(embeddedControl);
                    if (i != baseIndex)
                        RpgEE.sideNavListView.AddEmbeddedControl(embeddedControl, j, i - 1);
                }
            }

            ElementList.RemoveAt(baseIndex);

            RpgEE.map.renderMap();
        }

        /** Private helper method to sort out Zones by ID */
        private static Predicate<MapElement> FindByID(int ID)
        {
            return delegate (MapElement element)
            {
                return element.ID == ID;
            };
        }
    }
    public class MapZone : MapElement
    {
        private int[,] Data;

        /* Default fields */
        private int opacity = 150;

        public MapZone(Image map, List<MapElement> list) : base(map, list)
        {
            this.Image = new Bitmap(map.Width, map.Height);
            this.Brush = new SolidBrush(getRandomColor());

            this.Type = "Zone";
            this.Name = this.Type + this.ID;

            /* Create the 2D array to store the data held by the overlay Zone */
            this.Data = new int[map.Width/Map.blockSize + 1, map.Height/Map.blockSize + 1];

            /* */
            Generator<Form>.addMapListItem(RpgEE.sideNavListView, this);
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
            selectorHelper(Color.CornflowerBlue);
        }

        /** Public method to de-select a given Zone
         * It turns the background behind the Label back to white */
        public void unselectBackground()
        {
            selectorHelper(Color.Transparent);
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
            Button colorBtn = (Button)RpgEE.sideNavListView.GetEmbeddedControl(ListViewEx.colorIndex, this.getListIndex());
            colorBtn.BackColor = transparentColor;
        }

        /** Private helper method to handle Zone data selection/deselection */
        private void setDataHelper (Point pt, int x)
        {
            Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] = x;
        }

        /** Private helper method to toggle Background colors of selectors
         * Handles also view updates to finalise them */
        private void selectorHelper (Color newColor)
        {
            RpgEE.sideNavListView.GetEmbeddedControl(ListViewEx.nameIndex, this.getListIndex()).BackColor = newColor;
            RpgEE.sideNavListView.GetEmbeddedControl(ListViewEx.typeIndex, this.getListIndex()).BackColor = newColor;

            RpgEE.sideNavListView.Items[this.getListIndex()].BackColor = newColor;

            RpgEE.sideNavListView.Update();
        }

        /** Private color helper randomiser */
        private Color getRandomColor()
        {
            /* If opacity is != from 255 it is semi-transparent */
            return Color.FromArgb(opacity, RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256));
        }
    }
}

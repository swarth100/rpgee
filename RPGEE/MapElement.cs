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
    public abstract class MapElement
    {
        public Image Image { get; set; }
        public Brush Brush { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public bool Visible { get; set; }

        /* Private fields */
        protected int ID { get; }
        protected int opacity;
        private readonly List<MapElement> ElementList;

        public MapElement(Image map, List<MapElement> list)
        {
            this.ID = RpgEE.ZoneID++;
            this.Visible = true;

            /* Default image is the size of the map */
            this.Image = new Bitmap(map.Width, map.Height);

            /* Store a reference to the Object's reference list */
            this.ElementList = list;

            /* Append the new element to the list of zones */
            list.Add(this);
        }

        /* List operations. Index handling and find() */
        #region listOperations

        /** Public function to return a given Zone's list index based upon it's ID */
        public int getListIndex()
        {
            return ElementList.FindIndex(FindByID(this.ID));
        }

        /** Private helper method to sort out Zones by ID */
        private static Predicate<MapElement> FindByID(int ID)
        {
            return delegate (MapElement element)
            {
                return element.ID == ID;
            };
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
        #endregion

        /* Helper methods to set/unset Data Points */
        #region dataPoints

        /** Public method to retrieve the selection status of a given point in the DataMap */
        public abstract bool isPointSelected(Point pt);

        /** Public method to set a given Point of Data as selected in the DataMap */
        protected abstract void setDataPoint(Point pt, int x);

        /** Public method to set a specific Point's Zone data to true */
        public void addPoint(Graphics graphics, Point pt)
        {
            drawPointHelper(graphics, pt);
            setDataPoint(pt, 1);
        }

        /** Protected helper method to handle drawing to a MapElement's Image */
        protected abstract void drawPointHelper(Graphics graphics, Point pt);

        /** Public method to un-set a specific Point's Zone data, reverting it back to false */
        public void removePoint(Graphics graphics, Point pt)
        {
            removePointHelper(graphics, pt);
            setDataPoint(pt, 0);
        }

        protected abstract void removePointHelper(Graphics graphics, Point pt);

        #endregion

        /* Helper methods to change background of selection */
        #region backgroundColor

        /** Public method to select a given Zone
         * It turns the background behind the Label blue (when selected) */
        public void selectBackground()
        {
            selectorHelper(RpgEE.selectedColor);
        }

        /** Public method to de-select a given Zone
         * It turns the background behind the Label back to white */
        public void unselectBackground()
        {
            selectorHelper(Color.Transparent);
        }

        #endregion

        /* Helper methods for brush Color changes */
        #region brushColor

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

            /* Change the color of the whole existing Zone rendered onto the map */
            updateElementColor(transparentColor);
        }

        /** Protected method invoked to change the color of all points coloured in the Image */
        protected abstract void updateElementColor(Color newColor);

        /** Private helper method to toggle Background colors of selectors
         * Handles also view updates to finalise them */
        private void selectorHelper(Color newColor)
        {
            RpgEE.sideNavListView.GetEmbeddedControl(ListViewEx.nameIndex, this.getListIndex()).BackColor = newColor;
            RpgEE.sideNavListView.GetEmbeddedControl(ListViewEx.typeIndex, this.getListIndex()).BackColor = newColor;

            RpgEE.sideNavListView.Items[this.getListIndex()].BackColor = newColor;

            RpgEE.sideNavListView.Update();
        }

        #endregion

        /* Generic helper methods */
        #region helperMethods
        /** Helper method to erase a region (the size of a square blockSize) from a given zone's image
         * It iterates and removes every single pixel */
        protected void removeBitmapRegion(Point pt)
        {
            for (int i = 0; i < Map.blockSize; i++)
                for (int j = 0; j < Map.blockSize; j++)
                    (Image as Bitmap).SetPixel(pt.X + i, pt.Y + j, Color.Empty);
        }

        /** Helper method to generate a default name for the object */
        protected String defaultName()
        {
            return this.Type + this.ID;
        }

        /** Helper method to handle object addition to the sideNav's Menu */
        protected void addToGenerator()
        {
            /* Add the newly created element to the mapList's sideNav */
            Generator<Form>.addMapListItem(RpgEE.sideNavListView, this);
        }

        /** Private color helper randomiser */
        protected Color getRandomColor(int opacity)
        {
            /* If opacity is != from 255 it is semi-transparent */
            return Color.FromArgb(opacity, RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256));
        }

        #endregion
    }

    public class MapPoint : MapElement
    {
        private Point Position;
        private bool isPositionSet;
        private Object _PositionLock = new Object();

        public MapPoint(Image map, List<MapElement> list) : base(map, list)
        {
            this.opacity = 200;
            this.Brush = new SolidBrush(getRandomColor(opacity));

            this.Type = "Point";
            this.Name = this.defaultName();

            this.addToGenerator();
        }

        /* Asbtract methods overriden by concrete class implementation */
        #region overrideAbstract

        /** Helper method to set a given point's Data in the Object's DataMap
         * Because MapPoints store one point only, we update the point's position and accordingly set its value
         * to true or false */
        override
        protected void setDataPoint(Point pt, int x)
        {
            Position = pt;
            if (x == 0)
                isPositionSet = false;
            else
                isPositionSet = true;
        }

        /** Helper method to determine if the given point is the MapPoint's selected point */
        override
        public bool isPointSelected(Point pt)
        {
            return pt.Equals(Position) && isPositionSet;
        }

        /** Helper method to update the point's color */
        override
        protected void updateElementColor(Color newColor)
        {
            /* Erase the previously selected Point */
            RpgEE.map.erasePointNoRender(Position);

            /* Draw the new point selection with the new color */
            RpgEE.map.drawPoint(Position);
        }

        /** Helper method to draw a given point on the map.
         * It handles removal of the point at the previous position */
        override
        protected void drawPointHelper(Graphics graphics, Point pt)
        {
            /* Remove the previously drawn point before drawing the new one */
            RpgEE.map.erasePoint(Position);

            /* Points are draw as ellipses with an outer container rectangle the size of a single cell */
            graphics.FillEllipse(Brush, new Rectangle(pt, new Size(Map.blockSize, Map.blockSize)));
        }

        /** Helper method to remove a point from the Map */
        override
        protected void removePointHelper(Graphics graphics, Point pt)
        {
            removeBitmapRegion(pt);
        }

        #endregion
    }

    public class MapZone : MapElement
    {
        private int[,] Data;
        private Object _DataLock = new Object();
        private int DataWidth;
        private int DataHeight;

        public MapZone(Image map, List<MapElement> list) : base(map, list)
        {
            this.opacity = 150;
            this.Brush = new SolidBrush(getRandomColor(opacity));

            this.Type = "Zone";
            this.Name = this.defaultName();

            /* Create the 2D array to store the data held by the overlay Zone */
            lock (_DataLock)
            {
                this.DataWidth = map.Width / Map.blockSize + 1;
                this.DataHeight = map.Height / Map.blockSize + 1;
                this.Data = new int[DataWidth, DataHeight];
            }

            this.addToGenerator();
        }

        /* Asbtract methods overriden by concrete class implementation */
        #region overrideAbstract

        /** Public method to verify if a specific point's data has already been set */
        override
        public bool isPointSelected (Point pt)
        {
            lock (_DataLock)
                return Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] == 1;
        }

        /** Private helper method to handle Zone data selection/deselection */
        override
        protected void setDataPoint (Point pt, int x)
        {
            lock (_DataLock)
                Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] = x;
        }

        /** Private helper method to update the color of all points which are part of the Zone
         * on the Map */
        override
        protected void updateElementColor (Color newColor)
        {
            for (int i = 0; i < DataWidth * Map.blockSize; i += Map.blockSize)
            {
                for (int j = 0; j < DataHeight * Map.blockSize; j += Map.blockSize)
                {
                    Point pt = new Point(i, j);
                    if (isPointSelected(pt))
                    {
                        /* Each point must be removed from the Bitmap and redrawn with the new color */
                        RpgEE.map.erasePointNoRender(pt);
                        RpgEE.map.drawPointNoRender(pt);
                    }
                }
            }

            /* Forccefully update the map once complete */
            RpgEE.map.renderMap();
        }

        /** Private helper to draw a new Point, part of a Zone's region */
        override
        protected void drawPointHelper(Graphics graphics, Point pt)
        {
            graphics.FillRectangle(Brush, new Rectangle(pt, new Size(Map.blockSize, Map.blockSize)));
        }

        /** Private helper to remove a point from a Zone's rendered region */
        override
        protected void removePointHelper(Graphics graphics, Point pt)
        {
            removeBitmapRegion(pt);
        }

        #endregion
    }
}

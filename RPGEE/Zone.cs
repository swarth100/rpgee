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
        public String Name { get; }
        private int ID { get; }

        private readonly List<Zone> zones;

        private int[,] Data;

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

        public int getListIndex()
        {
            return zones.FindIndex(FindByID(this.ID));
        }

        public void addPoint (Point pt)
        {
            setDataHelper(pt, 1);
        }

        public bool isPointSelected (Point pt)
        {
            return Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] == 1;
        }

        public void removePoint(Point pt)
        {
            setDataHelper(pt, 0);
        }

        private void setDataHelper (Point pt, int x)
        {
            Data[pt.X / Map.blockSize, pt.Y / Map.blockSize] = x;
        }

        private static Predicate<Zone> FindByID (int ID)
        {
            return delegate (Zone zone)
            {
                return zone.ID == ID;
            };
        }

        private Color getRandomColor()
        {
            return Color.FromArgb(150, RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256), RpgEE.RandomGenerator.Next(256));
        }
    }
}

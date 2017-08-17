using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    public partial class RpgEE : Form
    {
        public RpgEE()
        {
            InitializeComponent();
            this.Text = "RpgEE";

            /** Generates a table to dock home Button Components
             * Table layout:
             * 
             * +----------+----------+----------+
             * |          |          |          |
             * |          |          |          |
             * |          |          |          |
             * +----------+----------+----------+
             * |          |          |          |
             * |          |          |          |
             * |          |          |          | 
             * +----------+----------+----------+ 
             */

            TableLayoutPanel homeTable = Generator<TableLayoutPanel>.generateHomeTable(this, 2, 3);

            Button mapBtn = Generator<Button>.addObject(new Button() { Text = "Map" }, homeTable, 0, 0);
            Button playersBtn = Generator<Button>.addObject(new Button() { Text = "Players" }, homeTable, 1, 0);
            Button areaBtn = Generator<Button>.addObject(new Button() { Text = "Area" }, homeTable, 2, 0);
            Button rulesBtn = Generator<Button>.addObject(new Button() { Text = "Rules" }, homeTable, 0, 1);
            Button spritesBtn = Generator<Button>.addObject(new Button() { Text = "Sprites" }, homeTable, 1, 1);
            Button customBtn = Generator<Button>.addObject(new Button() { Text = "Custom" }, homeTable, 2, 1);
           

            homeTable.ResumeLayout();
        }

        
    }
}

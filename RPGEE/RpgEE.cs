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
        /** Login Screen UI Components
         *  Initialised within RpgEE() Constructor
         */
        private readonly TableLayoutPanel loginTable;
        private readonly Label loginLbl;
        private readonly TextBox usernameTxt;
        private readonly TextBox passwordTxt;

        /** Home Screen UI Components
         *  Initialised within RpgEE() Constructor
         */
        private readonly TableLayoutPanel homeTable;
        private readonly Button mapBtn;
        private readonly Button playersBtn;
        private readonly Button areaBtn;
        private readonly Button rulesBtn;
        private readonly Button spritesBtn;
        private readonly Button customBtn;

        public RpgEE()
        {
            InitializeComponent();
            this.Text = "RpgEE";

            /** Generates a table to dock login Components
             * Table layout:
             * 
             * +-----+--------------------+-----+
             * |     |                    |     |
             * +-----+--------------------+-----+
             * |     |                    |     |
             * +-----+--------------------+-----+
             * |     |                    |     |
             * +-----+--------------------+-----+
             * |     |                    |     | 
             * +-----+--------------------+-----+
             * |     |                    |     | 
             * +-----+--------------------+-----+
             */

            loginTable = Generator<TableLayoutPanel>.generateLoginTable(this, 6, 3);
            loginTable.Visible = true;

            loginLbl = Generator<Label>.addObject(new Label() { Text = "Log Into EE", TextAlign = ContentAlignment.MiddleCenter }, loginTable, 1, 1);
            usernameTxt = Generator<TextBox>.addObject(new TextBox() { Text = "username" }, loginTable, 1, 3);
            passwordTxt = Generator<TextBox>.addObject(new TextBox() { Text = "password" }, loginTable, 1, 4);
            
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

            homeTable = Generator<TableLayoutPanel>.generateHomeTable(this, 2, 3);

            mapBtn = Generator<Button>.addObject(new Button() { Text = "Map" }, homeTable, 0, 0);
            playersBtn = Generator<Button>.addObject(new Button() { Text = "Players" }, homeTable, 1, 0);
            areaBtn = Generator<Button>.addObject(new Button() { Text = "Area" }, homeTable, 2, 0);
            rulesBtn = Generator<Button>.addObject(new Button() { Text = "Rules" }, homeTable, 0, 1);
            spritesBtn = Generator<Button>.addObject(new Button() { Text = "Sprites" }, homeTable, 1, 1);
            customBtn = Generator<Button>.addObject(new Button() { Text = "Custom" }, homeTable, 2, 1);
        }

        
    }
}

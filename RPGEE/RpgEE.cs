using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly TextBox roomIDTxt;
        private readonly Button loginBtn;

        /** Home Screen UI Components
         *  Initialised within RpgEE() Constructor
         */
        private readonly TableLayoutPanel homeTable;
        private readonly Button mapBtn;
        private readonly Button playersBtn;
        private readonly Button areaBtn;
        private readonly Button rulesBtn;
        private readonly Button spritesBtn;
        private readonly Button optionsBtn;

        /* General structures ad data */
        private Thread connectionThread;

        public RpgEE()
        {
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

            loginTable = Generator<TableLayoutPanel>.generateLoginTable(this, 8, 3);
            loginTable.Visible = true;

            loginLbl = Generator<Label>.addObject(new Label() { Text = "Log Into EE", TextAlign = ContentAlignment.MiddleCenter }, loginTable, 1, 1);
            usernameTxt = Generator<TextBox>.addObject(new TextBox() { Text = "username" }, loginTable, 1, 3);
            passwordTxt = Generator<TextBox>.addObject(new TextBox() { Text = "password" }, loginTable, 1, 4);
            roomIDTxt = Generator<TextBox>.addObject(new TextBox() { Text = "roomID" }, loginTable, 1, 5);

            /* Login button */
            loginBtn = Generator<Button>.addObject(new Button() { Text = "login" }, loginTable, 1, 6);
            loginBtn.Click += new System.EventHandler(this.loginBtn_Click);

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
            optionsBtn = Generator<Button>.addObject(new Button() { Text = "Options" }, homeTable, 2, 1);
        }

        /** Login btn click handler
         * When fired sets the appropriate Connection Details and fires the Connection bakcground thread */
        void loginBtn_Click(object sender, EventArgs e)
        {
            lock (ConnectionDetails._lock)
            {
                ConnectionDetails.username = usernameTxt.Text;
                ConnectionDetails.password = passwordTxt.Text;
                ConnectionDetails.roomID = roomIDTxt.Text;
            }
            
            /* Spawn connection thread, set it to background and run it */
            connectionThread = new Thread(BackgroundThread.runConnectionThread);
            connectionThread.IsBackground = true;
            connectionThread.Start();
        }
    }
}

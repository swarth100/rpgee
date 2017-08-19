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
        /* List of enums for possible screens in RpgEE */
        public enum Layers
        {
            Login,
            Home,
            Map,
            Players,
            Areas,
            Game,
            Sprites,
            Custom
        }

        /** Login Screen UI Components
         *  Initialised within RpgEE() Constructor
         */
        private static TableLayoutPanel loginTable;
        private readonly Label loginLbl;
        private readonly TextBox usernameTxt;
        private readonly TextBox passwordTxt;
        private readonly TextBox roomIDTxt;
        private readonly Button loginBtn;

        /** Home Screen UI Components
         *  Initialised within RpgEE() Constructor
         */
        private static TableLayoutPanel homeTable;
        private readonly Button mapBtn;
        private readonly Button playersBtn;
        private readonly Button areaBtn;
        private readonly Button rulesBtn;
        private readonly Button spritesBtn;
        private readonly Button optionsBtn;

        /** Map Screen UI Components
         *  Initialised within RpgEE() Constructor
         */
        private static TableLayoutPanel mapTable;
        public static PictureBox mapPct;
        private readonly Button backBtn;
        private readonly Button sideBtn;
        private readonly Button topBtn;

        /* General structures and data */
        private Thread connectionThread;
        private Thread computationThread;
        private static TableLayoutPanel currentScreen;
        private static Form RpgEEForm;

        public RpgEE()
        {
            this.Text = "RpgEE";
            this.Size = new Size(600, 400);
            RpgEEForm = this;

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

            /* Login table must originally be set visible */
            loginTable = Generator<TableLayoutPanel>.generateLoginTable(this, 8, 3);
            RpgEE.showScreen(Layers.Login);

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

            /* Map button */
            mapBtn = Generator<Button>.addObject(new Button() { Text = "Map" }, homeTable, 0, 0);
            mapBtn.Click += new System.EventHandler(this.mapBtn_Click);

            playersBtn = Generator<Button>.addObject(new Button() { Text = "Players" }, homeTable, 1, 0);
            areaBtn = Generator<Button>.addObject(new Button() { Text = "Area" }, homeTable, 2, 0);
            rulesBtn = Generator<Button>.addObject(new Button() { Text = "Rules" }, homeTable, 0, 1);
            spritesBtn = Generator<Button>.addObject(new Button() { Text = "Sprites" }, homeTable, 1, 1);
            optionsBtn = Generator<Button>.addObject(new Button() { Text = "Options" }, homeTable, 2, 1);

            /** Generates a table to dock map Button Components
             * Table layout:
             * 
             * +-----+---------------------+
             * |     |                     |
             * +-----+---------------------+
             * |     |                     |
             * |     |                     |
             * |     |                     | 
             * +-----+---------------------+ 
             */
            mapTable = Generator<TableLayoutPanel>.generateGeneralTable(this);
            mapPct = Generator<PictureBox>.addDraggablePictureBox(new DraggablePictureBox(), mapTable, 1, 1);

            /* Temporary button placeholders */
            backBtn = Generator<Button>.addObject(new Button() { Text = "Back" }, mapTable, 0, 0);
            topBtn = Generator<Button>.addObject(new Button() { Text = "RpgEE" }, mapTable, 1, 0);
            sideBtn = Generator<Button>.addObject(new Button() { Text = "SideNav" }, mapTable, 0, 1);

            /* Spawn computations thread, set it to background and run it */
            computationThread = new Thread(BackgroundThread.runComputationsThread);
            computationThread.IsBackground = true;
            computationThread.Start();

            /* Debug */
            //RpgEE.showScreen(Layers.Home);
        }

        #region btnClicks

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

        /** Map btn click handler
         * When fired changes the layout to the map appropriate one */
        void mapBtn_Click(object sender, EventArgs e)
        {
            RpgEE.showScreen(Layers.Map);
        }

        #endregion

        #region layerUpdates

        /** The following structure was generously inspired by:
         * https://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe */

        delegate void LayerUpdateCallback(Form form, TableLayoutPanel panel, bool status);

        public static void UpdateLayer(Form form, TableLayoutPanel panel, bool status)
        {
            /** InvokeRequired required compares the thread ID of the 
             * calling thread to the thread ID of the creating thread. 
             * If these threads are different, it returns true. */
            if (panel.InvokeRequired)
            {
                LayerUpdateCallback cb = new LayerUpdateCallback(UpdateLayer);
                form.Invoke(cb, new object[] { form, panel, status });
            }
            else
            {
                /* Changing a panel's visibility status must also update the currentScreen panel */
                panel.Visible = status;
                currentScreen = panel;
            }
        }

        /** Screen layer handler
         * Switches the viewed screen according to the given selection */
        public static void showScreen(Layers newScreen)
        {
            if (currentScreen != null)
            {
                RpgEE.UpdateLayer(RpgEEForm, currentScreen, false);
            }

            switch(newScreen)
            {
                case Layers.Login:
                    RpgEE.UpdateLayer(RpgEEForm, loginTable, true);
                    break;
                case Layers.Home:
                    RpgEE.UpdateLayer(RpgEEForm, homeTable, true);
                    break;
                case Layers.Map:
                    RpgEE.UpdateLayer(RpgEEForm, mapTable, true);
                    break;
            }
        }
        #endregion
    }
}

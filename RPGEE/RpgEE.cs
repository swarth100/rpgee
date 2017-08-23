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

        /* Public elements */

        /* Random generator */
        public static Random RandomGenerator = new Random();
        public static int ZoneID = 0;

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
        private readonly TableLayoutPanel sideNavTable;
        public static ListViewEx sideNavListView;
        public static Map map;
        private static Label loadLbl;
        private readonly TableLayoutPanel mapBtnTable1;
        private readonly TableLayoutPanel mapBtnTable2;
        private readonly Button moveMapBtn;
        private readonly Button drawMapBtn;
        private readonly Button inspectMapBtn;
        private readonly Button newMapBtn;
        private readonly Button newPinMapBtn;
        private readonly Button deleteMapBtn;
        private readonly Button backBtn;
        private readonly Button topBtn;

        /* General structures and data */
        private Thread connectionThread;
        private Thread computationThread;
        private static TableLayoutPanel currentScreen;
        private static Form RpgEEForm;

        public RpgEE()
        {
            this.Text = "RpgEE";
            this.Size = new Size(650, 450);
            RpgEEForm = this;

            #region loginLayout

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

            #endregion

            #region homeLayout

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

            #endregion

            #region mapLayout

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

            /* Initialise the MAP */
            map = new Map();
            map.setPictureBox(new DraggablePictureBox(map));

            /* Initialise the map's draggable image and placeholder */
            loadLbl = Generator<Label>.addObject(new Label() { Text = "Loading ...", TextAlign = ContentAlignment.MiddleCenter }, mapTable, 1, 1);

            /* Initialise sideNav Components */
            sideNavTable = Generator<TableLayoutPanel>.generateSideTable(3, 1);
            Generator<TableLayoutPanel>.addObject(sideNavTable, mapTable, 0, 1);

            sideNavListView = Generator<ListViewEx>.addObject(new ListViewEx(new[] { "Name", "Type", "Edit", "Color", "Show", "Remove" }), sideNavTable, 0, 0);

            /* Initialise mapButtons */
            mapBtnTable1 = Generator<TableLayoutPanel>.generateButtonTable(1, 5);
            Generator<TableLayoutPanel>.addObject(mapBtnTable1, sideNavTable, 1, 0);

            mapBtnTable2 = Generator<TableLayoutPanel>.generateButtonTable(1, 5);
            Generator<TableLayoutPanel>.addObject(mapBtnTable2, sideNavTable, 2, 0);

            moveMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.moveBtnImage },
                mapBtnTable1, 0, 0);
            moveMapBtn.Click += this.moveMapBtn_Click;

            drawMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.drawBtnImage },
                mapBtnTable1, 1, 0);
            drawMapBtn.Click += this.drawMapBtn_Click;

            inspectMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.inspectBtnImage },
                mapBtnTable1, 2, 0);
            inspectMapBtn.Click += this.inspectMapBtn_Click;

            deleteMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.eraseBtnImage },
                mapBtnTable1, 3, 0);
            deleteMapBtn.Click += this.deleteMapBtn_Click;

            /* Initialise second level of buttons */
            newPinMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.addBtnImage },
                mapBtnTable1, 4, 0);
            newPinMapBtn.Click += this.newPinMapBtn_Click;

            newMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.newBtnImage },
                mapBtnTable2, 0, 0);
            newMapBtn.Click += this.newMapBtn_Click;

            /* Temporary button placeholders */
            backBtn = Generator<Button>.addObject(new Button() { Text = "Back" }, mapTable, 0, 0);
            topBtn = Generator<Button>.addObject(new Button() { Text = "RpgEE" }, mapTable, 1, 0);

            #endregion

            /* Spawn computations thread, set it to background and run it */
            computationThread = new Thread(BackgroundThread.runComputationsThread);
            computationThread.IsBackground = true;
            computationThread.Start();

            /* Debug */
            // RpgEE.showScreen(Layers.Home);
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

        void moveMapBtn_Click(object sender, EventArgs e)
        {
            map.changeMapStatus(Map.Status.Move, sender as Control);
        }

        void drawMapBtn_Click(object sender, EventArgs e)
        {
            map.changeMapStatus(Map.Status.Draw, sender as Control);
        }

        void inspectMapBtn_Click(object sender, EventArgs e)
        {
            map.changeMapStatus(Map.Status.Inspect, sender as Control);
        }

        void newMapBtn_Click(object sender, EventArgs e)
        {
            map.addNewZone();
        }

        void deleteMapBtn_Click(object sender, EventArgs e)
        {
            map.changeMapStatus(Map.Status.Delete, sender as Control);
        }

        void newPinMapBtn_Click(object sender, EventArgs e)
        {
            map.changeMapStatus(Map.Status.Insert, sender as Control);
        }

        #endregion

        #region layerUpdates

        /** The following structure was generously inspired by:
         * https://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe */

        delegate void LayerUpdateCallback(Form form, TableLayoutPanel panel, bool status);

        private static void UpdateLayer(Form form, TableLayoutPanel panel, bool status)
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

        #region mapUpdates

        public static void spawnMap()
        {
            map.SpawnMap(RpgEEForm, mapTable, loadLbl);
        }

        public static void refreshMap()
        {
            map.RefreshMap(RpgEEForm, map.PictureBox);
        }

        #endregion
    }
}

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
            Options
        }

        /* Public elements */

        /* Colors */
        public static Color normalColor = Color.Transparent;
        public static Color selectedColor = Color.CornflowerBlue;

        /* Random generator */
        public static Random RandomGenerator = new Random();
        public static int ZoneID = 0;

        /* Login Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel loginTable;
        private readonly Label loginLbl;
        private readonly TextBox usernameTxt;
        private readonly TextBox passwordTxt;
        private readonly TextBox roomIDTxt;
        private readonly Button loginBtn;

        /* Home Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel homeTable;
        private readonly Button mapBtn;
        private readonly Button playersBtn;
        private readonly Button areaBtn;
        private readonly Button gameBtn;
        private readonly Button spritesBtn;
        private readonly Button optionsBtn;

        public static Button backBtn;
        public static Label topLbl;

        /* Map Screen UI Components Initialised within RpgEE() Constructor */
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

        /* Area Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel areaTable;

        /* Game Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel gameTable;

        /* Sprites Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel spritesTable;

        /* Player Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel playerTable;

        /* Custom Screen UI Components Initialised within RpgEE() Constructor */
        private static TableLayoutPanel customTable;

        /* General structures and data */
        private Thread connectionThread;
        private Thread computationThread;
        private static TableLayoutPanel currentScreen;
        private static Form RpgEEForm;

        public RpgEE()
        {
            this.Text = "RpgEE";
            this.Size = new Size(850, 550);
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
            loginBtn.Click += this.loginBtn_Click;

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
            mapBtn = Generator<Button>.addHomeButton(new Button() { Text = "Map" }, homeTable, 0, 0, Layers.Map);
            playersBtn = Generator<Button>.addHomeButton(new Button() { Text = "Players" }, homeTable, 1, 0, Layers.Players);
            areaBtn = Generator<Button>.addHomeButton(new Button() { Text = "Area" }, homeTable, 2, 0, Layers.Areas);
            gameBtn = Generator<Button>.addHomeButton(new Button() { Text = "Rules" }, homeTable, 0, 1, Layers.Game);
            spritesBtn = Generator<Button>.addHomeButton(new Button() { Text = "Sprites" }, homeTable, 1, 1, Layers.Sprites);
            optionsBtn = Generator<Button>.addHomeButton(new Button() { Text = "Options" }, homeTable, 2, 1, Layers.Options);

            /* General UI components */
            backBtn = new Button() { Text = "Back" };
            backBtn.Click += backBtn_Click;

            topLbl = new Label() { Text = "RpgEE", TextAlign = ContentAlignment.MiddleCenter };

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

            moveMapBtn = Generator<Button>.addStatusButton(new Button() { Image = Properties.Resources.moveBtnImage },
                mapBtnTable1, 0, 0, Map.Status.Move);

            drawMapBtn = Generator<Button>.addStatusButton(new Button() { Image = Properties.Resources.drawBtnImage },
                mapBtnTable1, 1, 0, Map.Status.Draw);

            inspectMapBtn = Generator<Button>.addStatusButton(new Button() { Image = Properties.Resources.inspectBtnImage },
                mapBtnTable1, 2, 0, Map.Status.Inspect);

            deleteMapBtn = Generator<Button>.addStatusButton(new Button() { Image = Properties.Resources.eraseBtnImage },
                mapBtnTable1, 3, 0, Map.Status.Delete);

            /* Initialise second level of buttons */
            newMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.newBtnImage },
                mapBtnTable2, 0, 0);
            newMapBtn.Click += this.newMapBtn_Click;

            newPinMapBtn = Generator<Button>.addObject(new Button() { Image = Properties.Resources.addBtnImage },
                mapBtnTable2, 1, 0);
            newPinMapBtn.Click += this.newPinMapBtn_Click;

            /* Select moveButton as default */
            moveMapBtn_Click(moveMapBtn, null);

            #endregion

            #region areaLayout

            areaTable = Generator<TableLayoutPanel>.generateGeneralTable(this);

            #endregion

            #region gameLayout

            gameTable = Generator<TableLayoutPanel>.generateGeneralTable(this);

            #endregion

            #region spritesLayout

            spritesTable = Generator<TableLayoutPanel>.generateGeneralTable(this);

            TableLayoutPanel leftSpritesTable = Generator<TableLayoutPanel>.generateSideTable(2, 1);
            Generator<TableLayoutPanel>.addObject(leftSpritesTable, spritesTable, 0, 1);

            TableLayoutPanel leftSpritesBtnTable = Generator<TableLayoutPanel>.generateButtonTable(1, 5);
            Generator<TableLayoutPanel>.addObject(leftSpritesBtnTable, leftSpritesTable, 0, 1);

            TableLayoutPanel tableSplitter = Generator<TableLayoutPanel>.generateRightSideTable();
            Generator<TableLayoutPanel>.addObject(tableSplitter, spritesTable, 1, 1);

            TableLayoutPanel rightSpritesTable = Generator<TableLayoutPanel>.generateSideTable(2, 1);
            Generator<TableLayoutPanel>.addObject(rightSpritesTable, tableSplitter, 1, 0);

            TableLayoutPanel rightSpritesBtnTable = Generator<TableLayoutPanel>.generateButtonTable(1, 5);
            Generator<TableLayoutPanel>.addObject(rightSpritesBtnTable, rightSpritesTable, 0, 1);


            /* Temporary button placeholders */
            Button btn1= Generator<Button>.addObject(new Button() { Text = "1" }, leftSpritesTable, 0, 0);
            Button btn2 = Generator<Button>.addObject(new Button() { Text = "2" }, leftSpritesBtnTable, 0, 0);
            Button btn3 = Generator<Button>.addObject(new Button() { Text = "3" }, leftSpritesBtnTable, 1, 0);
            Button btn4 = Generator<Button>.addObject(new Button() { Text = "4" }, tableSplitter, 0, 0);
            Button btn5 = Generator<Button>.addObject(new Button() { Text = "5" }, rightSpritesTable, 0, 0);
            Button btn6 = Generator<Button>.addObject(new Button() { Text = "6" }, rightSpritesBtnTable, 0, 0);
            Button btn7 = Generator<Button>.addObject(new Button() { Text = "7" }, rightSpritesBtnTable, 1, 0);

            #endregion

            #region playerLayout

            playerTable = Generator<TableLayoutPanel>.generateGeneralTable(this);

            #endregion

            #region customLayout

            customTable = Generator<TableLayoutPanel>.generateGeneralTable(this);

            #endregion

            /* Spawn computations thread, set it to background and run it */
            computationThread = new Thread(BackgroundThread.runComputationsThread);
            computationThread.IsBackground = true;
            computationThread.Start();

            /* Debug */
             RpgEE.showScreen(Layers.Home);
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
        void backBtn_Click(object sender, EventArgs e)
        {
            RpgEE.showScreen(Layers.Home);
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
            map.addNewPoint();
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
                    currentScreen = loginTable;
                    break;
                case Layers.Home:
                    currentScreen = homeTable;
                    break;
                case Layers.Map:
                    currentScreen = mapTable;
                    initialiseGeneralEmenets("Map");
                    break;
                case Layers.Areas:
                    currentScreen = areaTable;
                    initialiseGeneralEmenets("Area");
                    break;
                case Layers.Game:
                    currentScreen = gameTable;
                    initialiseGeneralEmenets("Game");
                    break;
                case Layers.Sprites:
                    currentScreen = spritesTable;
                    initialiseGeneralEmenets("Sprites");
                    break;
                case Layers.Players:
                    currentScreen = playerTable;
                    initialiseGeneralEmenets("Players");
                    break;
                case Layers.Options:
                    currentScreen = customTable;
                    initialiseGeneralEmenets("Custom");
                    break;
                default:
                    break;
            }

            RpgEE.UpdateLayer(RpgEEForm, currentScreen, true);
        }

        private static void initialiseGeneralEmenets(String name)
        {
            topLbl.Text = "RpgEE - " + name + " View";

            /* Generate the default Table UI components */
            Generator<Button>.addObject(backBtn, currentScreen, 0, 0);

            Generator<Label>.addObject(topLbl, currentScreen, 1, 0);
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

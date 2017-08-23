using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    class Generator<T> where T : Control
    {

        #region privateTableGenerators
        /** Private layout table generator.
         * Receives a row and column formatter functions, a containner form and numbers for rows and columns.
         * Creates a new TableLayoutPanel Object initialised accordingly.
         * The newly generated table is hid from the container form by default.
         *  */
        private static TableLayoutPanel generateTable(Action<TableLayoutPanel, int> rowFunc, Action<TableLayoutPanel, int> colFunc, int rows, int cols)
        {
            /* Initialises the table and row/column spacing */
            TableLayoutPanel layoutTable = new TableLayoutPanel();
            layoutTable.ColumnCount = cols;
            for (int i = 0; i < cols; i++)
            {
                colFunc(layoutTable, i);
            }

            layoutTable.RowCount = rows;
            for (int i = 0; i < rows; i++)
            {
                rowFunc(layoutTable, i);
            }

            layoutTable.Dock = DockStyle.Fill;

            return layoutTable;
        }

        /* */
        private static TableLayoutPanel generateTableForm(Action<TableLayoutPanel, int> rowFunc, Action<TableLayoutPanel, int> colFunc, Object container, int rows, int cols)
        {
            TableLayoutPanel panel = generateTable(rowFunc, colFunc, rows, cols);

            ((Form) container).Controls.Add(panel);

            /* Hides the table from the form, hence initialisation isn't rendered */
            panel.Visible = false;

            return panel;
        }

        #region homeTable

        /** Private helper function for home table column generation
         * Given a TableLayoutPanel generates 100% wide (equally spaced) columns
         *  */
        private static void generateHomeTableHelperCols(TableLayoutPanel layout, int i)
        {
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        }

        /** Private helper function for home table row generation
         * Given a TableLayoutPanel generates 100% wide (equally spaced) rows
         *  */
        private static void generateHomeTableHelperRows(TableLayoutPanel layout, int i)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        }

        #endregion

        #region loginTable

        /** Private helper function for login table column generation
         *  */
        private static void generateLoginTableHelperCols(TableLayoutPanel layout, int i)
        {
            System.Single value;
            switch (i)
            {
                case 0:
                case 2:
                    value = 20F;
                    break;
                default:
                    value = 80F;
                    break;
            }
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, value));
        }

        /** Private helper function for login table row generation
         *  */
        private static void generateLoginTableHelperRows(TableLayoutPanel layout, int i)
        {
            System.Single value;
            switch (i)
            {
                case 0:
                case 7:
                    value = 20F;
                    break;
                default:
                    value = 10F;
                    break;
            }
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, value));
        }

        #endregion

        #region generalTable

        /** Private helper function for a general component's table column generation
         *  */
        private static void generateGeneralTableHelperCols(TableLayoutPanel layout, int i)
        {
            System.Single value;
            switch (i)
            {
                case 0:
                    value = 250;
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, value));
                    break;
                default:
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    break;
            }
        }

        /** Private helper function for a general component's table row generation
         *  */
        private static void generateGeneralTableHelperRows(TableLayoutPanel layout, int i)
        {
            System.Single value;
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    value = 50;
                    layout.RowStyles.Add(new RowStyle(SizeType.Absolute, value));
                    break;
            }
        }

        #endregion

        #region sideTable

        /** Private helper function for a sideNav component's table column generation
         *  */
        private static void generateSideTableHelperCols(TableLayoutPanel layout, int i)
        {
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        }

        /** Private helper function for a sideNav component's table row generation
         *  */
        private static void generateSideTableHelperRows(TableLayoutPanel layout, int i)
        {
            System.Single value;
            switch (i)
            {
                case 0:
                    value = 100F;
                    layout.RowStyles.Add(new RowStyle(SizeType.Percent, value));
                    break;
                case 1:
                case 2:
                    value = 50;
                    layout.RowStyles.Add(new RowStyle(SizeType.Absolute, value));
                    break;
            }
        }

        #endregion

        #region buttonTable

        /** Private helper function for a map button's component's table column generation
         *  */
        private static void generateButtonTableHelperCols(TableLayoutPanel layout, int i)
        {
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        }

        /** Private helper function for a map button's component's table column generation
         *  */
        private static void generateButtonTableHelperRows(TableLayoutPanel layout, int i)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        }

        #endregion

        #endregion

        #region publicTableGenerators

        /** Public function to generate the Home Table */
        public static TableLayoutPanel generateHomeTable(Form container, int rows, int cols)
        {
            return generateTableForm(generateHomeTableHelperRows, generateHomeTableHelperCols, container, rows, cols);
        }

        /** Public function to generate the Home Table */
        public static TableLayoutPanel generateLoginTable(Form container, int rows, int cols)
        {
            return generateTableForm(generateLoginTableHelperRows, generateLoginTableHelperCols, container, rows, cols);
        }

        /** Public function to generate a General component Table */
        public static TableLayoutPanel generateGeneralTable(Form container)
        {
            return generateTableForm(generateGeneralTableHelperRows, generateGeneralTableHelperCols, container, 2, 2);
        }

        #endregion

        #region subTableGenerators

        public static TableLayoutPanel generateSideTable(int rows, int cols)
        {
            return generateTable(generateSideTableHelperRows, generateSideTableHelperCols, rows, cols);
        }

        public static TableLayoutPanel generateButtonTable(int rows, int cols)
        {
            return generateTable(generateButtonTableHelperRows, generateButtonTableHelperCols, rows, cols);
        }

        #endregion

        /** Public function to add and dock a new Object to a given TableLayoutPanel within a form
         * The newly created object must be assigned the right row and column indexes
         *  */
        public static T addObject(T obj, TableLayoutPanel table, int row, int col)
        {
            obj.Dock = DockStyle.Fill;
            table.Controls.Add(obj, row, col);
            return obj;
        }

        /** Public function to add and dock a DraggablePictureBox given a TableLayoutPanel within a form
         * The newly created object must be assigned the right row and column indexes
         *  */
        public static PictureBox addDraggablePictureBox(DraggablePictureBox obj, TableLayoutPanel table, int row, int col)
        {
            /* Generates a new sub level TableLayoutPanel */
            TableLayoutPanel mapHelper = Generator<TableLayoutPanel>.addObject(new TableLayoutPanel() , table, 2, 1);

            /* The table layout panel is just a placeholder, as it enforces no layout */
            mapHelper.Visible = true;
            mapHelper.SuspendLayout();

            /* The DraggablePictureBox is added to the sub-level layout panel with no enforced docking */
            Generator<PictureBox>.addObject(obj, mapHelper, 0, 0);
            obj.Dock = DockStyle.None;

            mapHelper.Resize += mapHelper_Resize;

            return obj;
        }

        private static void mapHelper_Resize(object sender, EventArgs e)
        {
            TableLayoutPanel mapHelper = sender as TableLayoutPanel;

            int deltaHeight = (mapHelper.Height - RpgEE.map.PictureBox.Height)/2;
            
            if (deltaHeight > 0)
            {
                RpgEE.map.PictureBox.Top = deltaHeight;
            }
            else if (RpgEE.map.PictureBox.Bounds.Y + RpgEE.map.PictureBox.Height < mapHelper.Height)
            {
                RpgEE.map.PictureBox.Top = mapHelper.Height - RpgEE.map.PictureBox.Height;
            }

            int deltaWidth = (mapHelper.Width - RpgEE.map.PictureBox.Width) / 2;

            if (deltaWidth > 0)
            {
                RpgEE.map.PictureBox.Left = deltaWidth;
            }
            else if (RpgEE.map.PictureBox.Bounds.X + RpgEE.map.PictureBox.Width < mapHelper.Width)
            {
                RpgEE.map.PictureBox.Left = mapHelper.Width - RpgEE.map.PictureBox.Width;
            }

        }

        /** Public function to add and dock a new ListViewItem into a given ListViewEx(tended) list.
         * The newly created object supports the Zone format for the Map screen of the Application */
        public static ListViewItem addMapListItem(ListViewEx list, MapElement obj)
        {
            /* Creates a new empty Item and adds it normally to the list */
            ListViewItem listItem = new ListViewItem(new String[list.Columns.Count]);
            list.Items.Add(listItem);

            int rowCount = list.Items.Count - 1;

            /* Custom field for the Name field */
            addListViewControl(new MapListLabel(list, obj) { Text = obj.Name },
                MapListLabel.nameLbl_Click, list, ListViewEx.nameIndex, rowCount);

            /* Custom field at index 1 for the Type of input */
            addListViewControl(new MapListLabel(list, obj) { Text = obj.Type },
                MapListLabel.nameLbl_Click, list, ListViewEx.typeIndex, rowCount);

            /* Custom field at index 2 contains the edit button */
            addListViewControl(new MapListButton(list, obj) { Text = "Edit" },
                MapListButton.editBtn_Click, list, ListViewEx.editIndex, rowCount);

            /* Custom field at index 3 contains the color picker button */
            addListViewControl(new MapListButton(list, obj) { BackColor = (obj.Brush as SolidBrush).Color },
                MapListButton.colorBtn_Click, list, ListViewEx.colorIndex, rowCount);

            /* Custom field at index 4 for the Checkbox (for selection) */
            addListViewControl(new MapListButton(list, obj) { Image = Properties.Resources.checkboxBtnImage },
                MapListButton.selectedBtn_Click, list, ListViewEx.showIndex, rowCount);

            /* Custom field at index 5 for the Checkbox (for selection) */
            addListViewControl(new MapListButton(list, obj) { Image = Properties.Resources.binBtnImage }, 
                MapListButton.binBtn_Click, list, ListViewEx.removeIndex, rowCount);

            return listItem;
        }

        /** Private helper function to link ClickEvents to Controls.
         * It also adds the given Controls to the Extended ListView in the correct cell */
        private static void addListViewControl(Control btn, EventHandler click, ListViewEx list, int col, int row)
        {
            btn.Click += click;
            list.AddEmbeddedControl(btn, col, row);
        }

        #region listButton

        #endregion
    }
}

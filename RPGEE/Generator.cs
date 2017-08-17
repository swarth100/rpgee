using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    class Generator<T> where T : Control
    {
        /** Private layout table generator.
         * Receives a row and column formatter functions, a containner form and numbers for rows and columns.
         * Creates a new TableLayoutPanel Object initialised accordingly.
         * The newly generated table is hid from the container form by default.
         *  */
        private static TableLayoutPanel generateTable(Action<TableLayoutPanel, int> rowFunc, Action<TableLayoutPanel, int> colFunc, Form container, int rows, int cols)
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

            /* Adds the table to the form */
            container.Controls.Add(layoutTable);

            /* Hides the table from the form, hence initialisation isn't rendered */
            layoutTable.Visible = false;

            return layoutTable;
        }

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
                case 2:
                case 5:
                    value = 20F;
                    break;
                default:
                    value = 10F;
                    break;
            }
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, value));
        }

        /** Public function to generate the Home Table */
        public static TableLayoutPanel generateHomeTable(Form container, int rows, int cols)
        {
            return generateTable(generateHomeTableHelperRows, generateHomeTableHelperCols, container, rows, cols);
        }

        /** Public function to generate the Home Table */
        public static TableLayoutPanel generateLoginTable(Form container, int rows, int cols)
        {
            return generateTable(generateLoginTableHelperRows, generateLoginTableHelperCols, container, rows, cols);
        }

        /** Public function to add and dock a new Object to a given TableLayoutPanel within a form
         * The newly created object must be assigned the right row and column indexes
         *  */
        public static T addObject(T obj, TableLayoutPanel table, int row, int col)
        {
            obj.Dock = DockStyle.Fill;
            table.Controls.Add(obj, row, col);
            return obj;
        }
    }
}

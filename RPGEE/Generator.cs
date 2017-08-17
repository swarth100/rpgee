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
        public static TableLayoutPanel generateHomeTable(Form container, int rows, int cols)
        {
            TableLayoutPanel layoutTable = new TableLayoutPanel();
            layoutTable.ColumnCount = cols;
            for (int i = 0; i < cols; i++)
            {
                layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            }
            layoutTable.RowCount = rows;
            for (int i = 0; i < rows; i++)
            {
                layoutTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }
            layoutTable.Dock = DockStyle.Fill;

            /** Adds the table to the form */
            container.Controls.Add(layoutTable);

            return layoutTable;
        }

        public static T addObject(T obj, TableLayoutPanel table, int row, int col)
        {
            obj.Dock = DockStyle.Fill;
            table.Controls.Add(obj, row, col);
            return obj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    class Generator
    {
        public static TableLayoutPanel generateTable(int rows, int cols)
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

            return layoutTable;
        }
    }
}

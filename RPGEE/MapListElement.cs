using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    public class MapListLabel : Label
    {
        /* Public fields */
        public Object ListParent { get; }

        /* Private fields */
        private int RowIndex { get; }
        private ListViewEx List { get; }

        /* Constructor */
        public MapListLabel(ListViewEx list, Object parent, int index) : base()
        {
            ListParent = parent;
            RowIndex = index;
            List = list;

            TextAlign = ContentAlignment.MiddleCenter;
        }

        #region btnClicks

        /** Handles navBar name label click events
         * The events must fire back onto the button's parent */
        public static void nameLbl_Click(object sender, EventArgs e)
        {
            MapListLabel label = sender as MapListLabel;

            RpgEE.map.changeSelectedZone((label.ListParent as Zone).getListIndex());
        }

        #endregion

    }

    /** Public class which holds a reference to a number of fields for buttons inside the navList
         * Handles a number of UI interactions */
    public class MapListButton : Button
    {
        /* Public fields */
        public Object ListParent { get; }

        /* Private fields */
        private int RowIndex { get; }
        private ListViewEx List { get; }

        /* Constructor */
        public MapListButton(ListViewEx list, Object parent, int index) : base()
        {
            ListParent = parent;
            RowIndex = index;
            List = list;
        }

        /** Public method invoked by edit buttons.
         * Replaces the Label Name of the field with a TextBox.
         * Losing Focus of the TextBox will finalise the option written inside */
        public void replaceLabel()
        {
            TextBox editBox = new TextBox() { Text = (ListParent as Zone).Name };

            /* Add focus event handlers */
            editBox.LostFocus += editBox_LostFocus;

            replaceHelper(editBox);

            /* Must be placed at the end as the replaceHelper plays around with the Focus */
            editBox.Focus();
        }

        /** Private method to revert the ListItem's name back to a Label
         * Finalises the contents of the TextBox before removing it */
        private void replaceTextBox()
        {
            (ListParent as Zone).Name = List.GetEmbeddedControl(0, RowIndex).Text;
            Label nameLbl = new Label() { Text = (ListParent as Zone).Name, TextAlign = ContentAlignment.MiddleCenter };

            replaceHelper(nameLbl);
        }

        /* Helper method to replace the contents of the ListItem's first component */
        private void replaceHelper(Control newCtrl)
        {
            /* Remove the component present in the first cell */
            List.RemoveEmbeddedControl(List.GetEmbeddedControl(0, RowIndex));

            /* Add the newly generated component */
            List.AddEmbeddedControl(newCtrl, 0, RowIndex);
        }

        /* Handles a lose focus event for the ListItem's name editBox */
        private void editBox_LostFocus(object sender, EventArgs e)
        {
            replaceTextBox();
        }

        #region btnClicks

        /** Handles navBar edit button click events
         * The events must fire back onto the button's parent */
        public static void editBtn_Click(object sender, EventArgs e)
        {
            MapListButton button = sender as MapListButton;

            button.replaceLabel();
        }

        #endregion
    }
}

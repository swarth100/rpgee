using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPGEE
{
    public class MapListLabel : Label
    {
        /* Public fields */
        public Object ListParent { get; }

        /* Private fields */
        private ListViewEx List { get; }

        /* Constructor */
        public MapListLabel(ListViewEx list, Object parent) : base()
        {
            ListParent = parent;
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
        private ListViewEx List { get; }

        /* Constructor */
        public MapListButton(ListViewEx list, Object parent) : base()
        {
            ListParent = parent;
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
            Zone zone = (ListParent as Zone);
            zone.Name = List.GetEmbeddedControl(0, zone.getListIndex()).Text;
            Label nameLbl = new Label() { Text = (ListParent as Zone).Name, TextAlign = ContentAlignment.MiddleCenter };

            replaceHelper(nameLbl);
        }

        /* Helper method to replace the contents of the ListItem's first component */
        private void replaceHelper(Control newCtrl)
        {
            /* Remove the component present in the first cell */
            List.RemoveEmbeddedControl(List.GetEmbeddedControl(ListViewEx.nameIndex, (ListParent as Zone).getListIndex()));

            /* Add the newly generated component */
            List.AddEmbeddedControl(newCtrl, ListViewEx.nameIndex, (ListParent as Zone).getListIndex());
        }

        /** Helper method to spawn a new ColorDialog */
        private void spawnColorDialog()
        {
            ColorDialog MyDialog = new ColorDialog();
            /* Keeps the user from selecting a custom color. */
            MyDialog.AllowFullOpen = false;
            /* Allows the user to get help. (The default is false.) */
            MyDialog.ShowHelp = true;
            /* Sets the initial color select to the current text color. */
            MyDialog.Color = BackColor;

            /* Update the text box color if the user clicks OK */
            if (MyDialog.ShowDialog() == DialogResult.OK)
                (ListParent as Zone).changeColor(MyDialog.Color);
        }

        private void toggleSelected()
        {
            Zone parentZone = (ListParent as Zone);
            parentZone.Visible = !parentZone.Visible;

            Button refBtn = (List.GetEmbeddedControl(ListViewEx.showIndex, (ListParent as Zone).getListIndex()) as Button);

            if (parentZone.Visible)
                refBtn.Image = Properties.Resources.checkboxBtnImage;
            else
                refBtn.Image = null;
            
            RpgEE.map.renderMap();
        }

        private void removeItem()
        {
            (ListParent as Zone).remove();
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

        public static  void colorBtn_Click(object sender, System.EventArgs e)
        {
            MapListButton button = sender as MapListButton;

            button.spawnColorDialog();
        }

        public static void selectedBtn_Click(object sender, System.EventArgs e)
        {
            MapListButton button = sender as MapListButton;

            button.toggleSelected();
        }

        public static void binBtn_Click(object sender, System.EventArgs e)
        {
            MapListButton button = sender as MapListButton;

            button.removeItem();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSA_XPEC_editor
{
	public partial class ViewItemForm : Form
	{
		//The opened item's index
		int _itemIndex;

		//The list box on the main form
		ListBox _lb;

		public ViewItemForm(ListBox lb, int itemIndex)
		{
			InitializeComponent();							//Managed by Visual Studio

			_itemIndex = itemIndex;
			_lb = lb;

			txtData.Text = lb.Items[itemIndex].ToString();	//Assign the text in the textbox to the currently viewed string
		}

		//Triggered when you press "Save"
		private void SaveItem(object sender, EventArgs e)
		{
			_lb.Items[_itemIndex] = txtData.Text;			//Set the item in the listbox to the edited text
			this.Close();									//Close the form
		}

		//Triggered when you press "Close"
		private void Cancel(object sender, EventArgs e)
		{
			this.Close();									//Close the form
		}
	}
}

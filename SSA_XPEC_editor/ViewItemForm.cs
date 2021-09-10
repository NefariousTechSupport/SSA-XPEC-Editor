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
		int _itemIndex;
		ListBox _lb;
		public ViewItemForm(ListBox lb, int itemIndex)
		{
			InitializeComponent();
			_itemIndex = itemIndex;
			_lb = lb;

			txtData.Text = lb.Items[itemIndex].ToString();
		}

		private void SaveItem(object sender, EventArgs e)
		{
			_lb.Items[_itemIndex] = txtData.Text;
			this.Close();
		}

		private void Cancel(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}

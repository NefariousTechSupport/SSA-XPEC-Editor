using System;
using System.IO;
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
	public partial class Form1 : Form
	{
		LXBFile lxb;

		public Form1()
		{
			InitializeComponent();
		}
		void OpenFile(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "lxb files (*.lxb)|*.lxb|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 2;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					saveToolStripMenuItem.Enabled = true;
					lxb = new LXBFile(openFileDialog.FileName);
					this.Text = $"SSA XPEC Text Editor - v0.01 - \"{Path.GetFileName(openFileDialog.FileName)}\"";
					lbText.Items.Clear();
					for(uint i = 0; i < lxb.numberOfItems; i++)
					{
						lbText.Items.Add(lxb.ReadString(lxb.addresses[i]));
					}
				}
			}
		}
		void ViewData(object sender, MouseEventArgs e)
		{
			if (lxb == null) return;
			int index = lbText.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
			{
				ViewItemForm viewItem = new ViewItemForm(lbText, index);

				viewItem.Show();
			}
		}
		/*void PreviewData(object sender, MouseEventArgs e)
		{
			if (lxb == null) return;
			int index = lbText.IndexFromPoint(e.Location);
			if(index != ListBox.NoMatches)
			{
				tt_lbText.Show(lbText.Items[index].ToString(), Owner);
			}
		}*/
		private void SaveFile(object sender, EventArgs e)
		{
			lxb.Save(lbText.Items.OfType<string>().ToArray());
		}
	}
}

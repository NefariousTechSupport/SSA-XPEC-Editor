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
		//The main lxb file
		LXBFile lxb;

		//The main apk file
		APKFile apk;

		//The main save data file
		DATFile dat;

		public Form1()
		{
			InitializeComponent();		//Set up the Form, managed by Visual Studio
		}
		void OpenFile(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "lxb files (*.lxb)|*.lxb|All files (*.*)|*.*";		//Only allow lxb files, with the option for all files just in case anyone wants that
				openFileDialog.FilterIndex = 2;												//We want 2 filters
				openFileDialog.RestoreDirectory = true;										//Basically remember what folder you were in last time

				if (openFileDialog.ShowDialog() == DialogResult.OK)			//If the user selects a file
				{
					this.Text = $"SSA XPEC Text Editor - v0.01 - \"{Path.GetFileName(openFileDialog.FileName)}\"";	//Change the window title
					switch (Path.GetExtension(openFileDialog.FileName))
					{
						case ".lxb":
							lxb = new LXBFile(openFileDialog.FileName);														//Load the selected file as an lxb file

							lbText.Items.Clear();												//Remove all previously loaded items
							if(lxb.attributes.Any(x => x == "TextTable"))						//If this is a text file
							{
								for(uint i = 0; i < lxb.numberOfItems; i++)						//For every text item
								{
									lbText.Items.Add(lxb.ReadString(lxb.itemAddresses[i]));		//Read the text item and add it to the listbox
								}

								saveToolStripMenuItem.Enabled = true;				//Enable saving if this is a text file
							}
							else
							{
								saveToolStripMenuItem.Enabled = false;				//Disable saving if this is not a text file, done to prevent corrupting non-text files
							}
							attributesToolStripMenuItem.Enabled = true;				//Enable viewing attributes
							break;
						case ".dat":
							
							break;
						/*case ".apk":
							lxb = null;
							
							apk = new APKFile(openFileDialog.FileName);
							break;*/
						default:
							break;
					}
				}
			}
		}

		//Triggered when double clicking an item in lbText
		void ViewData(object sender, MouseEventArgs e)
		{
			if (lxb == null) return;										//If there is no file loaded, cancel.
			int index = lbText.IndexFromPoint(e.Location);					//Figure out what file was double clicked
			if (index != ListBox.NoMatches)									//If a file was confirmed to be loaded
			{
				ViewItemForm viewItem = new ViewItemForm(lbText, index);	//Create a form where you can view and edit text
				viewItem.Show();											//Show said form
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

		//Triggered when pressing "File > Save"
		private void SaveFile(object sender, EventArgs e)
		{
			lxb.Save(lbText.Items.OfType<string>().ToArray());				//Just save lol
		}

		//Triggered when pressing "View > Attributes"
		private void ViewAttributes(object sender, EventArgs e)
		{
			ViewAttributesForm viewAttributes = new ViewAttributesForm(lxb);	//Create and set up a form where you can view attributes
			viewAttributes.Show();												//Show said form
		}
	}
}

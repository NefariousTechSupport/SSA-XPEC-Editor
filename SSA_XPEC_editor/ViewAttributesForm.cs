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
	public partial class ViewAttributesForm : Form
	{
		LXBFile _lxb;

		public ViewAttributesForm(LXBFile lxb)
		{
			InitializeComponent();
			_lxb = lxb;
			for (int i = 0; i < _lxb.numberOfAttributes; i++)
			{
				lbAttributes.Items.Add(_lxb.attributes[i]);
			}
		}
		
	}
}

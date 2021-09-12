
namespace SSA_XPEC_editor
{
	partial class ViewAttributesForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbAttributes = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lbAttributes
			// 
			this.lbAttributes.FormattingEnabled = true;
			this.lbAttributes.Location = new System.Drawing.Point(13, 13);
			this.lbAttributes.Name = "lbAttributes";
			this.lbAttributes.Size = new System.Drawing.Size(227, 420);
			this.lbAttributes.TabIndex = 0;
			// 
			// ViewAttributesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(258, 449);
			this.Controls.Add(this.lbAttributes);
			this.Name = "ViewAttributesForm";
			this.Text = "Attributes";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbAttributes;
	}
}
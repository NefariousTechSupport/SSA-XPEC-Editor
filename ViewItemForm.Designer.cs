
namespace SSA_XPEC_editor
{
	partial class ViewItemForm
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
			this.txtData = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtData
			// 
			this.txtData.Location = new System.Drawing.Point(13, 12);
			this.txtData.Multiline = true;
			this.txtData.Name = "txtData";
			this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtData.Size = new System.Drawing.Size(375, 162);
			this.txtData.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(313, 180);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.Cancel);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(232, 180);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.SaveItem);
			// 
			// ViewItemForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 215);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.txtData);
			this.Name = "ViewItemForm";
			this.Text = "ViewItemForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtData;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
	}
}
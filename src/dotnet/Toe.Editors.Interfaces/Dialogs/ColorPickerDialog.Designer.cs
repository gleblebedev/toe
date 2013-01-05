namespace Toe.Editors.Interfaces.Dialogs
{
	partial class ColorPickerDialog
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
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.rgbPanel = new System.Windows.Forms.Panel();
			this.editA = new Toe.Editors.Interfaces.Views.EditByteView();
			this.label4 = new System.Windows.Forms.Label();
			this.editB = new Toe.Editors.Interfaces.Views.EditByteView();
			this.label3 = new System.Windows.Forms.Label();
			this.editG = new Toe.Editors.Interfaces.Views.EditByteView();
			this.label2 = new System.Windows.Forms.Label();
			this.editR = new Toe.Editors.Interfaces.Views.EditByteView();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.originalColor = new Toe.Editors.Interfaces.Views.ColorView();
			this.newColor = new Toe.Editors.Interfaces.Views.ColorView();
			this.rgbPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(328, 114);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(247, 114);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// rgbPanel
			// 
			this.rgbPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rgbPanel.Controls.Add(this.editA);
			this.rgbPanel.Controls.Add(this.label4);
			this.rgbPanel.Controls.Add(this.editB);
			this.rgbPanel.Controls.Add(this.label3);
			this.rgbPanel.Controls.Add(this.editG);
			this.rgbPanel.Controls.Add(this.label2);
			this.rgbPanel.Controls.Add(this.editR);
			this.rgbPanel.Controls.Add(this.label1);
			this.rgbPanel.Location = new System.Drawing.Point(13, 39);
			this.rgbPanel.Name = "rgbPanel";
			this.rgbPanel.Size = new System.Drawing.Size(390, 29);
			this.rgbPanel.TabIndex = 2;
			// 
			// editA
			// 
			this.editA.Location = new System.Drawing.Point(314, 0);
			this.editA.Name = "editA";
			this.editA.Size = new System.Drawing.Size(69, 20);
			this.editA.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(293, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(14, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "A";
			// 
			// editB
			// 
			this.editB.Location = new System.Drawing.Point(218, 0);
			this.editB.Name = "editB";
			this.editB.Size = new System.Drawing.Size(69, 20);
			this.editB.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(197, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "B";
			// 
			// editG
			// 
			this.editG.Location = new System.Drawing.Point(122, 0);
			this.editG.Name = "editG";
			this.editG.Size = new System.Drawing.Size(69, 20);
			this.editG.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(101, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(15, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "G";
			// 
			// editR
			// 
			this.editR.Location = new System.Drawing.Point(26, 0);
			this.editR.Name = "editR";
			this.editR.Size = new System.Drawing.Size(69, 20);
			this.editR.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(15, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "R";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.originalColor, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.newColor, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(390, 29);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// originalColor
			// 
			this.originalColor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.originalColor.Location = new System.Drawing.Point(3, 3);
			this.originalColor.Name = "originalColor";
			this.originalColor.Size = new System.Drawing.Size(189, 23);
			this.originalColor.TabIndex = 0;
			// 
			// newColor
			// 
			this.newColor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.newColor.Location = new System.Drawing.Point(198, 3);
			this.newColor.Name = "newColor";
			this.newColor.Size = new System.Drawing.Size(189, 23);
			this.newColor.TabIndex = 1;
			// 
			// ColorPickerDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(415, 149);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.rgbPanel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Name = "ColorPickerDialog";
			this.Text = "ColorPickerDialog";
			this.rgbPanel.ResumeLayout(false);
			this.rgbPanel.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Panel rgbPanel;
		private Views.EditByteView editA;
		private System.Windows.Forms.Label label4;
		private Views.EditByteView editB;
		private System.Windows.Forms.Label label3;
		private Views.EditByteView editG;
		private System.Windows.Forms.Label label2;
		private Views.EditByteView editR;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Views.ColorView originalColor;
		private Views.ColorView newColor;
	}
}
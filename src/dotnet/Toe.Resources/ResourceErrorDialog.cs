using System.Windows.Forms;

namespace Toe.Resources
{
	public class ResourceErrorDialog:Form
	{
		private Button button1;
		private CheckBox checkBox1;
		private Label label1;
		private Label label2;

		private static bool skipAlways;

		public ResourceErrorDialog(string filePath, string message)
		{
			this.InitializeComponent();
			this.label1.Text = string.Format("Can't read {0}", filePath);
			this.label2.Text = message;
		}


		public static DialogResult ShowDialogOrDefault(string filePath, string message)
		{
			if (skipAlways) return DialogResult.Ignore;
			var d = new ResourceErrorDialog(filePath, message);
			return d.ShowDialog();
		}

		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(386, 77);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Skip";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(13, 77);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(82, 17);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "Skip always";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "label2";
			// 
			// ResourceErrorDialog
			// 
			this.ClientSize = new System.Drawing.Size(473, 112);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button1);
			this.Name = "ResourceErrorDialog";
			this.Text = "Resource error";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Ignore;
			if (this.checkBox1.Checked) skipAlways = true;
		}
	}
}
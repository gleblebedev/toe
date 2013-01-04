using System;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Dialogs;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class EditResourceReferenceView:UserControl, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		private readonly ICommandHistory history;

		private TableLayoutPanel table;
		private Button buttonEdit;
		private Button btnOpen;
		DataContextContainer dataContext = new DataContextContainer();

		private uint type;

		public EditResourceReferenceView()
		{
			InitializeComponent();
			this.dataContext.PropertyChanged += OnReferencePropertyChanged;
			this.dataContext.DataContextChanged += OnReferencePropertyChanged;
			this.buttonEdit.Click += ChooseResource;
			this.btnOpen.Click += OpenResourceEditor;
		}

		protected virtual void ChooseResource(object sender, EventArgs e)
		{
			var d = new ResourcePickerDialog(resourceManager, Reference.Type);
			if (d.ShowDialog() == DialogResult.OK)
			{
				SetNewValue(d.SelectedHash,d.Selected);
			}
		}

		protected virtual void SetNewValue(uint selectedHash, object selectedVal)
		{
			history.SetValue(Reference, Reference.HashReference, selectedHash, (a, b) => a.HashReference = b);
		}

		public EditResourceReferenceView(IEditorEnvironment editorEnvironment, IResourceManager resourceManager, ICommandHistory history)
			: this()
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.history = history;
		}

		protected virtual void OpenResourceEditor(object sender, EventArgs a)
		{
			var filePath = this.Reference.FilePath;
			if (!string.IsNullOrEmpty(filePath))
			{
				editorEnvironment.Open(filePath);
				return;
			}
			ChooseResource(sender, a);
		}

		private void OnReferencePropertyChanged(object sender, EventArgs e)
		{
			this.buttonEdit.Text = string.Format("{0}", this.Reference);
			if (this.Reference.Resource == null && !this.Reference.IsEmpty)
			{
				this.BackColor = Color.Red;
			}
			else
			{
				this.BackColor = Color.FromArgb(255, 240, 240, 240);
			}
		}

		#region Implementation of IView

		public ResourceReference Reference
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return this.dataContext.Value as ResourceReference;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public uint Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		#endregion

		private void InitializeComponent()
		{
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.buttonEdit = new System.Windows.Forms.Button();
			this.btnOpen = new System.Windows.Forms.Button();
			this.table.SuspendLayout();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.table.Controls.Add(this.buttonEdit, 0, 0);
			this.table.Controls.Add(this.btnOpen, 1, 0);
			this.table.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Name = "table";
			this.table.RowCount = 1;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Size = new System.Drawing.Size(150, 30);
			this.table.TabIndex = 0;
			// 
			// buttonEdit
			// 
			this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonEdit.Location = new System.Drawing.Point(3, 3);
			this.buttonEdit.Name = "buttonEdit";
			this.buttonEdit.Size = new System.Drawing.Size(114, 24);
			this.buttonEdit.TabIndex = 0;
			this.buttonEdit.UseVisualStyleBackColor = true;
			// 
			// btnOpen
			// 
			this.btnOpen.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnOpen.Location = new System.Drawing.Point(123, 3);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(24, 24);
			this.btnOpen.TabIndex = 1;
			this.btnOpen.Text = ">";
			this.btnOpen.UseVisualStyleBackColor = true;
			// 
			// EditResourceReferenceView
			// 
			this.Controls.Add(this.table);
			this.Name = "EditResourceReferenceView";
			this.Size = new System.Drawing.Size(150, 30);
			this.table.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
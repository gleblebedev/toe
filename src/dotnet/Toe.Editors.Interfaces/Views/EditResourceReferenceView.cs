using System;
using System.Drawing;
using System.Windows.Forms;

using Autofac;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Dialogs;
using Toe.Resources;

namespace Toe.Editors.Interfaces.Views
{
	public class EditResourceReferenceView : UserControl, IView
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly bool fileReferencesAllowed;

		private readonly ICommandHistory history;

		private readonly IResourceManager resourceManager;

		private Button btnOpen;

		private Button buttonEdit;

		private TableLayoutPanel table;

		#endregion

		#region Constructors and Destructors

		public EditResourceReferenceView()
		{
			this.InitializeComponent();
			this.dataContext.PropertyChanged += this.OnReferencePropertyChanged;
			this.dataContext.DataContextChanged += this.OnReferencePropertyChanged;
			this.buttonEdit.Click += this.ChooseResource;
			this.btnOpen.Click += this.OpenResourceEditor;
		}

		public EditResourceReferenceView(
			IEditorEnvironment editorEnvironment,
			IResourceManager resourceManager,
			ICommandHistory history,
			IComponentContext context,
			bool fileReferencesAllowed)
			: this()
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.history = history;
			this.context = context;
			this.fileReferencesAllowed = fileReferencesAllowed;
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public ResourceReference Reference
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return this.dataContext.Value as ResourceReference;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		public uint Type
		{
			get
			{
				ResourceReference resourceReference = this.Reference;
				if (resourceReference == null)
				{
					return 0;
				}
				return resourceReference.Type;
			}
		}

		#endregion

		#region Methods

		protected virtual void ChooseResource(object sender, EventArgs e)
		{
			var d = new ResourcePickerDialog(
				this.resourceManager, this.editorEnvironment, this.Reference.Type, this.fileReferencesAllowed);
			if (d.ShowDialog() == DialogResult.OK)
			{
				this.SetNewValue(d.SelectedItem);
			}
		}

		protected virtual void OpenResourceEditor(object sender, EventArgs a)
		{
			ResourceReference resourceReference = this.Reference;

			if (resourceReference == null || resourceReference.IsEmpty)
			{
				this.ChooseResource(sender, a);
				return;
			}

			var filePath = resourceReference.FilePath;
			if (!string.IsNullOrEmpty(filePath))
			{
				this.editorEnvironment.Open(filePath);
				return;
			}

			var r = this.resourceManager.ConsumeResource(resourceReference.Type, resourceReference.HashReference);
			var file = r.Source;
			this.resourceManager.ReleaseResource(resourceReference.Type, resourceReference.HashReference);

			if (file != null && !string.IsNullOrEmpty(file.FilePath))
			{
				this.editorEnvironment.Open(file.FilePath);
				return;
			}
		}

		protected virtual void SetNewValue(IResourceItem item)
		{
			var t = this.context.ResolveKeyed<IResourceType>(this.Type);

			var newR = t.BuildReference(item, this.fileReferencesAllowed);
			var oldR = this.Reference.Clone();
			var setReferenceCommand = new SetReferenceCommand(this.Reference, newR, oldR);
			this.history.RegisterAction(setReferenceCommand);
			setReferenceCommand.Redo();
		}

		private void InitializeComponent()
		{
			this.table = new TableLayoutPanel();
			this.buttonEdit = new Button();
			this.btnOpen = new Button();
			this.table.SuspendLayout();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			this.table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			this.table.Controls.Add(this.buttonEdit, 0, 0);
			this.table.Controls.Add(this.btnOpen, 1, 0);
			this.table.Dock = DockStyle.Fill;
			this.table.Location = new Point(0, 0);
			this.table.Name = "table";
			this.table.RowCount = 1;
			this.table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			this.table.Size = new Size(150, 30);
			this.table.TabIndex = 0;
			// 
			// buttonEdit
			// 
			this.buttonEdit.Dock = DockStyle.Fill;
			this.buttonEdit.Location = new Point(3, 3);
			this.buttonEdit.Name = "buttonEdit";
			this.buttonEdit.Size = new Size(114, 24);
			this.buttonEdit.TabIndex = 0;
			this.buttonEdit.UseVisualStyleBackColor = true;
			// 
			// btnOpen
			// 
			this.btnOpen.Dock = DockStyle.Fill;
			this.btnOpen.Location = new Point(123, 3);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new Size(24, 24);
			this.btnOpen.TabIndex = 1;
			this.btnOpen.Text = ">";
			this.btnOpen.UseVisualStyleBackColor = true;
			// 
			// EditResourceReferenceView
			// 
			this.Controls.Add(this.table);
			this.Name = "EditResourceReferenceView";
			this.Size = new Size(150, 30);
			this.table.ResumeLayout(false);
			this.ResumeLayout(false);
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

		#endregion

		protected class SetReferenceCommand : ICommand
		{
			#region Constants and Fields

			private readonly ResourceReference newR;

			private readonly ResourceReference oldR;

			private readonly ResourceReference target;

			#endregion

			#region Constructors and Destructors

			public SetReferenceCommand(ResourceReference target, ResourceReference newR, ResourceReference oldR)
			{
				this.target = target;
				this.newR = newR;
				this.oldR = oldR;
			}

			#endregion

			#region Public Methods and Operators

			public void Redo()
			{
				this.target.CopyFrom(this.newR);
			}

			public void Undo()
			{
				this.target.CopyFrom(this.oldR);
			}

			#endregion
		}
	}
}
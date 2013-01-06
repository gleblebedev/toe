using System;
using System.Windows.Forms;

using Toe.Resources;

namespace Toe.Editors.Interfaces.Dialogs
{
	public partial class ResourcePickerDialog : Form
	{
		#region Constants and Fields

		private readonly IEditorEnvironment editorEnvironment;

		private readonly bool fileReferencesAllowed;

		private readonly IResourceManager resourceManager;

		private readonly uint type;

		#endregion

		#region Constructors and Destructors

		public ResourcePickerDialog(
			IResourceManager resourceManager, IEditorEnvironment editorEnvironment, uint type, bool fileReferencesAllowed)
		{
			this.resourceManager = resourceManager;
			this.editorEnvironment = editorEnvironment;
			this.type = type;
			this.fileReferencesAllowed = fileReferencesAllowed;
			this.InitializeComponent();

			foreach (var i in resourceManager.GetAllResourcesOfType(type))
			{
				this.list.Items.Add(i);
			}
			if (fileReferencesAllowed)
			{
				this.btnOpenFile.Visible = true;
			}
		}

		#endregion

		#region Public Properties

		public IResourceItem SelectedItem
		{
			get
			{
				if (this.list.SelectedItems.Count == 0)
				{
					return null;
				}
				return ((IResourceItem)this.list.SelectedItems[0]);
			}
		}

		#endregion

		#region Methods

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void btnOpenFile_Click(object sender, EventArgs e)
		{
			var d = new OpenFileDialog();
			if (DialogResult.OK == d.ShowDialog())
			{
				string safeFileName = d.FileName;
				this.editorEnvironment.Open(safeFileName);

				var f = this.resourceManager.EnsureFile(safeFileName);
				if (f != null)
				{
					foreach (var i in f.Items)
					{
						if (i.Type == this.type)
						{
							this.list.Items.Add(i);
							this.list.SelectedItem = i;
							this.DialogResult = DialogResult.OK;
						}
					}
				}
			}
		}

		#endregion
	}
}
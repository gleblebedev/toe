using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Resources;

namespace Toe.Editors.Interfaces.Dialogs
{
	public partial class ResourcePickerDialog : Form
	{
		private readonly IResourceManager resourceManager;

		private readonly IEditorEnvironment editorEnvironment;

		private readonly uint type;

		private readonly bool fileReferencesAllowed;

		public ResourcePickerDialog(IResourceManager resourceManager,IEditorEnvironment editorEnvironment, uint type, bool fileReferencesAllowed)
		{
			this.resourceManager = resourceManager;
			this.editorEnvironment = editorEnvironment;
			this.type = type;
			this.fileReferencesAllowed = fileReferencesAllowed;
			InitializeComponent();
			
			foreach (var i in resourceManager.GetAllResourcesOfType(type))
			{
				list.Items.Add(i);
			} 
			if (fileReferencesAllowed)
			{
				btnOpenFile.Visible = true;
			}
		}
		public IResourceItem SelectedItem
		{
			get
			{
				if (list.SelectedItems.Count == 0) return null;
				return ((IResourceItem)list.SelectedItems[0]);
			}
		}
		
		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnOpenFile_Click(object sender, EventArgs e)
		{
			var d = new OpenFileDialog();
			if (DialogResult.OK == d.ShowDialog())
			{
				string safeFileName = d.FileName;
				editorEnvironment.Open(safeFileName);

				var f = resourceManager.EnsureFile(safeFileName);
				if (f != null)
				{
					foreach (var i in f.Items)
					{
						if(i.Type == type)
						{
							list.Items.Add(i);
							list.SelectedItem = i;
							this.DialogResult = DialogResult.OK;
						}
					}
				}
			}
		}
	}
}

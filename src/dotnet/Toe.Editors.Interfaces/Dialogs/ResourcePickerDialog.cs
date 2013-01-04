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

		public ResourcePickerDialog(IResourceManager resourceManager, uint type)
		{
			this.resourceManager = resourceManager;
			InitializeComponent();
			
			foreach (var i in resourceManager.GetAllResourcesOfType(type))
			{
				list.Items.Add(i);
			} 
		}

		public uint SelectedHash
		{
			get
			{
				return ((IResourceItem)list.SelectedItems[0]).Hash;
			}
		}
		public object Selected
		{
			get
			{
				return ((IResourceItem)list.SelectedItems[0]).Value;
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
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Resources;

namespace Toe.Editor
{
	public partial class AddNewItemForm : Form
	{
		private readonly IEditorOptions<AddNewItemFormOptions> editorOptions;

		public AddNewItemForm(IEditorOptions<AddNewItemFormOptions> editorOptions, IEnumerable<IResourceFileFormat> resourceFileFormats)
		{
			this.editorOptions = editorOptions;
			InitializeComponent();
			var directoryHistory = this.Options.DirectoryHistory;
			if (directoryHistory.Count == 0)
			{
				this.txtFolder.Text = Directory.GetCurrentDirectory();
			}
			else
			{
				this.txtFolder.Text = directoryHistory[directoryHistory.Count-1];
				var autoCompleteSource = new AutoCompleteStringCollection();
				autoCompleteSource.AddRange(directoryHistory.ToArray());
				this.txtFolder.AutoCompleteCustomSource = autoCompleteSource;
				this.txtFolder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				this.txtFolder.AutoCompleteSource = AutoCompleteSource.CustomSource;
			}

			if (resourceFileFormats != null)
			{
				foreach (var fileFormat in resourceFileFormats)
				{
					
				}
			}
		}

		public AddNewItemFormOptions Options
		{
			get
			{
				return this.editorOptions.Options;
			}
		}

		private void OnCreateClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.SaveDirectory();
			this.editorOptions.Save();
		}

		private void SaveDirectory()
		{
			var directoryHistory = this.Options.DirectoryHistory;
			var selectedPath = this.txtFolder.Text;
			for (int index = 0; index < directoryHistory.Count; index++)
			{
				var path = directoryHistory[index];
				if (path == selectedPath)
				{
					directoryHistory.RemoveAt(index);
					directoryHistory.Add(selectedPath);
					return;
				}
			}
			directoryHistory.Add(selectedPath);
			while (directoryHistory.Count > 10)
			{
				directoryHistory.RemoveAt(0);
			}
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void OnBrowseClick(object sender, EventArgs e)
		{
			var d = new FolderBrowserDialog() { SelectedPath = txtFolder.Text };
			if (d.ShowDialog() == DialogResult.OK)
			{
				txtFolder.Text = d.SelectedPath;
			}
		}
	}
}

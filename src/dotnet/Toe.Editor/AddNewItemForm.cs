using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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

		public AddNewItemForm(IEditorOptions<AddNewItemFormOptions> editorOptions, IEnumerable<IResourceEditorFactory> resourceFileFormats)
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
					ListViewGroup group = null;
					foreach (var format in fileFormat.SupportedFormats)
					{
						if (format.CanCreate)
						{
							if (group == null)
							{
								group = new ListViewGroup(fileFormat.Name);
								listView1.Groups.Add(group);
							}
							listView1.Items.Add(new ListViewItem(format.Name, group) { Tag = format });
						}
					}
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

		public string FilePath
		{
			get
			{
				return Path.Combine(txtFolder.Text, textBox1.Text);
			}
		}

		private void OnCreateClick(object sender, EventArgs e)
		{
			var fileFormatInfo = SelectedFormat;
			if (fileFormatInfo == null)
				return;

			var name = string.IsNullOrEmpty(textBox1.Text) ? string.Empty : Path.GetFileName(textBox1.Text);
			if (!fileFormatInfo.Extensions.Any(ffExt => name.EndsWith(ffExt, StringComparison.InvariantCultureIgnoreCase)))
			{
				if (MessageBox.Show("Extension is not supported by file format!", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
					return;
			}
			
			if (File.Exists(FilePath))
			{
				if (MessageBox.Show("File already exists! Overwrite?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;
			}

			var tempFileName = Path.GetTempFileName();
			using (var s = File.Create(tempFileName))
			{
				fileFormatInfo.Create(FilePath, s);
				s.Close();
			}
			File.Copy(tempFileName,FilePath,true);
			File.Delete(tempFileName);
			

			this.SaveDirectory();
			this.editorOptions.Save();

			DialogResult = DialogResult.OK;
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

		private bool isDefaultName = true;

		public IFileFormatInfo SelectedFormat
		{
			get
			{
				if (listView1.SelectedItems == null || listView1.SelectedItems.Count == 0)
					return null;
				return listView1.SelectedItems[0].Tag as IFileFormatInfo;
			}
		}
		private void OnSelectedItemChanged(object sender, EventArgs e)
		{
			var fileFormatInfo = SelectedFormat;
			if (fileFormatInfo == null)
				return;

			var dir = string.IsNullOrEmpty(textBox1.Text)?null:Path.GetDirectoryName(textBox1.Text);
			var name = string.IsNullOrEmpty(textBox1.Text) ? string.Empty : Path.GetFileNameWithoutExtension(textBox1.Text);
			var ext = string.IsNullOrEmpty(textBox1.Text) ? string.Empty:Path.GetExtension(textBox1.Text);

			if (isDefaultName)
			{
				name = Path.GetFileNameWithoutExtension(fileFormatInfo.DefaultFileName);
				ext = Path.GetExtension(fileFormatInfo.DefaultFileName);
				if (string.IsNullOrEmpty(ext)) ext = fileFormatInfo.Extensions[0];
				if (File.Exists(Path.Combine(txtFolder.Text,name+ext)))
				{
					int i = 1;
					while (File.Exists(Path.Combine(txtFolder.Text, string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", name, i, ext))))
					{
						++i;
					}
					name = string.Format(CultureInfo.InvariantCulture, "{0}{1}", name, i);
				}
			}
			else
			{
				ext = fileFormatInfo.Extensions[0];
			}

			var newFileName = name + ext;
			this.textBox1.Text = newFileName;
		}
	}
}

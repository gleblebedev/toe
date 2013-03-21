using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Toe.Editors.Interfaces;

namespace Toe.Editor
{
	public partial class AddNewItemForm : Form
	{
		#region Constants and Fields

		private readonly IEditorOptions<AddNewItemFormOptions> editorOptions;

		private bool isDefaultName = true;

		#endregion

		#region Constructors and Destructors

		public AddNewItemForm(
			IEditorOptions<AddNewItemFormOptions> editorOptions, IEnumerable<IResourceEditorFactory> resourceFileFormats)
		{
			this.editorOptions = editorOptions;
			this.InitializeComponent();
			var directoryHistory = this.Options.DirectoryHistory;
			if (directoryHistory.Count == 0)
			{
				this.txtFolder.Text = Directory.GetCurrentDirectory();
			}
			else
			{
				this.txtFolder.Text = directoryHistory[directoryHistory.Count - 1];
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
								this.listView1.Groups.Add(group);
							}
							this.listView1.Items.Add(new ListViewItem(format.Name, group) { Tag = format });
						}
					}
				}
			}
		}

		#endregion

		#region Public Properties

		public string FilePath
		{
			get
			{
				return Path.Combine(this.txtFolder.Text, this.textBox1.Text);
			}
		}

		public AddNewItemFormOptions Options
		{
			get
			{
				return this.editorOptions.Options;
			}
		}

		public IFileFormatInfo SelectedFormat
		{
			get
			{
				if (this.listView1.SelectedItems == null || this.listView1.SelectedItems.Count == 0)
				{
					return null;
				}
				return this.listView1.SelectedItems[0].Tag as IFileFormatInfo;
			}
		}

		#endregion

		#region Methods

		private void OnBrowseClick(object sender, EventArgs e)
		{
			var d = new FolderBrowserDialog { SelectedPath = this.txtFolder.Text };
			if (d.ShowDialog() == DialogResult.OK)
			{
				this.txtFolder.Text = d.SelectedPath;
			}
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void OnCreateClick(object sender, EventArgs e)
		{
			var fileFormatInfo = this.SelectedFormat;
			if (fileFormatInfo == null)
			{
				return;
			}

			var name = string.IsNullOrEmpty(this.textBox1.Text) ? string.Empty : Path.GetFileName(this.textBox1.Text);
			if (!fileFormatInfo.Extensions.Any(ffExt => name.EndsWith(ffExt, StringComparison.InvariantCultureIgnoreCase)))
			{
				if (MessageBox.Show("Extension is not supported by file format!", "Warning", MessageBoxButtons.OKCancel)
				    != DialogResult.OK)
				{
					return;
				}
			}

			if (File.Exists(this.FilePath))
			{
				if (MessageBox.Show("File already exists! Overwrite?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					return;
				}
			}

			var tempFileName = Path.GetTempFileName();
			using (var s = File.Create(tempFileName))
			{
				fileFormatInfo.Create(this.FilePath, s);
				s.Close();
			}
			File.Copy(tempFileName, this.FilePath, true);
			File.Delete(tempFileName);

			this.SaveDirectory();
			this.editorOptions.Save();

			this.DialogResult = DialogResult.OK;
		}

		private void OnSelectedItemChanged(object sender, EventArgs e)
		{
			var fileFormatInfo = this.SelectedFormat;
			if (fileFormatInfo == null)
			{
				return;
			}

			var dir = string.IsNullOrEmpty(this.textBox1.Text) ? null : Path.GetDirectoryName(this.textBox1.Text);
			var name = string.IsNullOrEmpty(this.textBox1.Text)
			           	? string.Empty
			           	: Path.GetFileNameWithoutExtension(this.textBox1.Text);
			var ext = string.IsNullOrEmpty(this.textBox1.Text) ? string.Empty : Path.GetExtension(this.textBox1.Text);

			if (this.isDefaultName)
			{
				name = Path.GetFileNameWithoutExtension(fileFormatInfo.DefaultFileName);
				ext = Path.GetExtension(fileFormatInfo.DefaultFileName);
				if (string.IsNullOrEmpty(ext))
				{
					ext = fileFormatInfo.Extensions[0];
				}
				if (File.Exists(Path.Combine(this.txtFolder.Text, name + ext)))
				{
					int i = 1;
					while (
						File.Exists(
							Path.Combine(this.txtFolder.Text, string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", name, i, ext))))
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

		#endregion
	}
}
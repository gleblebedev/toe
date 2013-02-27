using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autofac;

using Toe.Editors;
using Toe.Editors.Interfaces;
using Toe.Resources;

namespace Toe.Editor
{
	public partial class MainEditorWindow : Form
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IEditorOptions<MainEditorWindowOptions> options;

		private readonly IResourceEditor lastEditor;

		#endregion

		#region Constructors and Destructors

		public MainEditorWindow(IComponentContext context, IEditorOptions<MainEditorWindowOptions> options)
		{
			this.context = context;
			this.options = options;
			this.InitializeComponent();
			this.fileTabs.SelectedIndexChanged += this.RebindToEditor;

			if (options.Options.RecentFiles == null)
			{
				options.Options.RecentFiles = new ObservableCollection<string>();
			}

			options.Options.RecentFiles.CollectionChanged += ResetRecentFilesMenu;
			ResetRecentFilesMenu(options.Options.RecentFiles, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

		}

		private void ResetRecentFilesMenu(object sender, NotifyCollectionChangedEventArgs e)
		{
			recentFilesMenu.DropDownItems.Clear();
			foreach (var file in options.Options.RecentFiles)
			{
				recentFilesMenu.DropDownItems.Add(this.CreateRecentFileMenuItem(file));
			}
		}

		private ToolStripMenuItem CreateRecentFileMenuItem(string file)
		{
			return new ToolStripMenuItem(Path.GetFileName(file), null, (s, a) => this.OpenFile(file));
		}

		#endregion

		#region Public Methods and Operators

		public void OpenFile(string filename)
		{
			this.AddFileToRecentFiles(filename);
			foreach (TabPage fileTab in this.fileTabs.TabPages)
			{
				var tag = fileTab.Tag as IResourceEditor;
				if (tag != null)
				{
					if (string.Compare(tag.CurrentFileName, filename, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						this.fileTabs.SelectedTab = fileTab;
						return;
					}
				}
			}

			var resourceEditor = this.CreateEditor(filename);
			if (resourceEditor == null)
			{
				return;
			}

			resourceEditor.Control.Dock = DockStyle.Fill;
			resourceEditor.LoadFile(filename);

			var tabPage = new TabPage(Path.GetFileName(filename));
			tabPage.Controls.Add(resourceEditor.Control);
			tabPage.Tag = resourceEditor;
			this.fileTabs.TabPages.Add(tabPage);
			this.fileTabs.SelectedTab = tabPage;
		}

		private void AddFileToRecentFiles(string filename)
		{
			var f = Path.GetFullPath(filename);
			var i = this.options.Options.RecentFiles.IndexOf(f);
			if (i == 0)
				return;
			if (i > 0)
			{
				this.options.Options.RecentFiles.Move(i,0);
				this.options.Save();
				return;
			}
			this.options.Options.RecentFiles.Insert(0,f);
			while (this.options.Options.RecentFiles.Count > 10)
				this.options.Options.RecentFiles.RemoveAt(10);
			this.options.Save();
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			//var filename = @"C:\Marmalade\6.2\examples\games\kartz\data\environment\track_01\track_01.group";
			//OpenFile(@"C:\Marmalade\6.2\examples\IwGraphics\data\scalablePipeline\bike.group");
			//OpenFile(@"C:\GitHub\toe\testcontent\FunkyShader.itx");
			//OpenFile(@"C:\GitHub\toe\testcontent\FunkyVicGLES2.mtl");
		}

		private void BindEditor(IResourceEditor editor)
		{
			var notifyPropertyChanged = editor as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += this.EditorPropertyChanged;
			}
			this.UpdateMenuButtons(editor);
		}

		private IResourceEditor CreateEditor(string filename)
		{
			foreach (var ef in this.context.Resolve<IEnumerable<IResourceEditorFactory>>())
			{
				var e = ef.CreateEditor(filename);
				if (e != null)
				{
					return e;
				}
			}
			return new DefaultEditor(this.context.Resolve<IEditorEnvironment>());
		}

		private void EditorPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.UpdateMenuButtons(sender as IResourceEditor);
		}

		private void OnCloseMenuOption(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
			{
				return;
			}
			this.fileTabs.TabPages.Remove(tab);
			tab.Dispose();
		}

		private void OnEditMenuOption(object sender, EventArgs e)
		{
			this.Close();
		}

		private void OnOpenMenuOption(object sender, EventArgs e)
		{
			var fd = new OpenFileDialog();

			fd.Filter = this.BuildFilter();

			var r = fd.ShowDialog();
			if (r == DialogResult.OK)
			{
				foreach (string f in fd.FileNames)
				{
					this.OpenFile(f);
				}
			}
		}

		private string BuildFilter()
		{
			var editorFactories = this.context.Resolve<IEnumerable<IResourceEditorFactory>>();
			StringBuilder filterBuilder = new StringBuilder();

			List<string> ex = (from format in editorFactories from f in format.SupportedFormats from ext in f.Extensions select ext).ToList();
			if (ex.Count > 0)
			{
				BuildFilterOption(new FileFormatInfo() { Extensions = ex, Name = "All supported types" }, filterBuilder);
				foreach (
					var resourceFileFormat in editorFactories.SelectMany(resourceFileFormat => resourceFileFormat.SupportedFormats)
					)
				{
					filterBuilder.Append("|");
					BuildFilterOption(resourceFileFormat, filterBuilder);
				}
				filterBuilder.Append("|");
			}
			BuildFilterOption(new FileFormatInfo(){Extensions = new string[]{".*"}, Name = "All"}, filterBuilder);

			var filter = filterBuilder.ToString();
			return filter;
		}

		private static void BuildFilterOption(IFileFormatInfo format, StringBuilder allBuilder)
		{
			allBuilder.Append(format.Name);
			allBuilder.Append(" (");
			string separator = String.Empty;
			foreach (var ex in format.Extensions)
			{
				allBuilder.Append(separator);
				allBuilder.Append("*");
				allBuilder.Append(ex);
				separator = ", ";
			}
			allBuilder.Append(")|");
			separator = String.Empty;
			foreach (var ex in format.Extensions)
			{
				allBuilder.Append(separator);
				allBuilder.Append("*");
				allBuilder.Append(ex);
				separator = ";";
			}
		}

		private void OnSaveMenuOption(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
			{
				return;
			}
		}

		private void RebindToEditor(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
			{
				return;
			}
			var editor = tab.Tag as IResourceEditor;
			if (editor == null)
			{
				return;
			}

			if (this.lastEditor != editor)
			{
				this.UnbindEditor(this.lastEditor);
				this.BindEditor(editor);
			}
		}

		private void RedoClick(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
			{
				return;
			}
			var editor = tab.Tag as IResourceEditor;
			if (editor == null)
			{
				return;
			}
			editor.Redo();
		}

		private void UnbindEditor(IResourceEditor editor)
		{
			var notifyPropertyChanged = editor as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.EditorPropertyChanged;
			}
		}

		private void UndoClick(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
			{
				return;
			}
			var editor = tab.Tag as IResourceEditor;
			if (editor == null)
			{
				return;
			}
			editor.Undo();
		}

		private void UpdateMenuButtons(IResourceEditor editor)
		{
			this.undoMenuItem.Enabled = this.undoButton.Enabled = editor.CanUndo;
			this.redoButton.Enabled = this.redoButton.Enabled = editor.CanRedo;
		}

		private void combatEvagroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.OpenFile(@"C:\GitHub\toe\src\marmalade\data\male_lod0.group");
		}

		#endregion

		private void OnNewMenuClick(object sender, EventArgs e)
		{
			var d = context.Resolve<AddNewItemForm>();
			if (d.ShowDialog() == DialogResult.OK)
			{
				
			}
		}
	}
}
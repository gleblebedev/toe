using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

using Autofac;

using Toe.Editors;
using Toe.Editors.Interfaces;

namespace Toe.Editor
{
	public partial class MainEditorWindow : Form
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IResourceEditor lastEditor;

		#endregion

		#region Constructors and Destructors

		public MainEditorWindow(IComponentContext context)
		{
			this.context = context;
			this.InitializeComponent();
			this.fileTabs.SelectedIndexChanged += this.RebindToEditor;
		}

		#endregion

		#region Public Methods and Operators

		public void OpenFile(string filename)
		{
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
			var r = fd.ShowDialog();
			if (r == DialogResult.OK)
			{
				foreach (string f in fd.FileNames)
				{
					this.OpenFile(f);
				}
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
	}
}
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
		private readonly IComponentContext context;

		private IResourceEditor lastEditor = null;

		#region Constructors and Destructors

		public MainEditorWindow(Autofac.IComponentContext context )
		{
			this.context = context;
			this.InitializeComponent();
			fileTabs.SelectedIndexChanged += RebindToEditor;

		}

		private void RebindToEditor(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
				return;
			var editor = tab.Tag as IResourceEditor;
			if (editor == null)
				return;

			if (lastEditor != editor)
			{
				UnbindEditor(lastEditor);
				BindEditor(editor);
			}

		}

		private void BindEditor(IResourceEditor editor)
		{
			var notifyPropertyChanged = editor as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += EditorPropertyChanged;
			}
			UpdateMenuButtons(editor);
		}

		private void EditorPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			UpdateMenuButtons(sender as IResourceEditor);
		}

		private void UpdateMenuButtons(IResourceEditor editor)
		{
			undoMenuItem.Enabled = undoButton.Enabled = editor.CanUndo;
			redoButton.Enabled = redoButton.Enabled = editor.CanRedo;
		}

		private void UnbindEditor(IResourceEditor editor)
		{
			var notifyPropertyChanged = editor as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= EditorPropertyChanged;
			}
		}

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			//var filename = @"C:\Marmalade\6.2\examples\games\kartz\data\environment\track_01\track_01.group";
			//OpenFile(@"C:\Marmalade\6.2\examples\IwGraphics\data\scalablePipeline\bike.group");
			//OpenFile(@"C:\GitHub\toe\testcontent\FunkyShader.itx");
			//OpenFile(@"C:\GitHub\toe\testcontent\FunkyVicGLES2.mtl");
		}

		public void OpenFile(string filename)
		{
			foreach (TabPage fileTab in fileTabs.TabPages)
			{
				var tag = fileTab.Tag as IResourceEditor;
				if (tag != null)
				{
					if (string.Compare(tag.CurrentFileName, filename, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						fileTabs.SelectedTab = fileTab;
						return;
					}
				}
			}

			var resourceEditor = CreateEditor(filename);
			if (resourceEditor == null)
				return;

			resourceEditor.Control.Dock = DockStyle.Fill;
			resourceEditor.LoadFile(filename);

			var tabPage = new TabPage(Path.GetFileName(filename));
			tabPage.Controls.Add(resourceEditor.Control);
			tabPage.Tag = resourceEditor;
			this.fileTabs.TabPages.Add(tabPage);
			fileTabs.SelectedTab = tabPage;
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
			return new DefaultEditor(context.Resolve<IEditorEnvironment>());
		}

		#endregion

		private void OnEditMenuOption(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void OnOpenMenuOption(object sender, System.EventArgs e)
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

		private void OnSaveMenuOption(object sender, System.EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
				return;
		}

		private void OnCloseMenuOption(object sender, System.EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
				return;
			fileTabs.TabPages.Remove(tab);
			tab.Dispose();
		}

		private void combatEvagroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.OpenFile(@"C:\GitHub\toe\src\marmalade\data\male_lod0.group");
		}

		private void UndoClick(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
				return;
			var editor = tab.Tag as IResourceEditor;
			if (editor == null)
				return;
			editor.Undo();
		}

		private void RedoClick(object sender, EventArgs e)
		{
			var tab = this.fileTabs.SelectedTab;
			if (tab == null)
				return;
			var editor = tab.Tag as IResourceEditor;
			if (editor == null)
				return;
			editor.Redo();
		}

	
	}
}
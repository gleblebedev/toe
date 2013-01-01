using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Resources;

namespace Toe.Editors.Marmalade
{
	public class ResourceFileEditor : UserControl, IResourceEditor
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		private SplitContainer itemsPropertiesSplitter;

		private IResourceFile resourceFile;

		#endregion

		#region Constructors and Destructors

		public ResourceFileEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.Dock = DockStyle.Fill;

			this.InitializeComponent();
			this.InitializeEditor();
		}

		#endregion

		#region Public Properties

		public bool CanCopy
		{
			get
			{
				return false;
			}
		}

		public bool CanCut
		{
			get
			{
				return false;
			}
		}

		public bool CanPaste
		{
			get
			{
				return false;
			}
		}

		public bool CanRedo
		{
			get
			{
				return false;
			}
		}

		public bool CanUndo
		{
			get
			{
				return false;
			}
		}

		public Control Control
		{
			get
			{
				return this;
			}
		}

		public string CurrentFileName
		{
			get
			{
				return this.resourceFile.FilePath;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void CloseFile()
		{
			if (this.resourceFile != null)
			{
				this.resourceFile.Close();
				this.dataContext.Value = null;
				this.resourceFile = null;
			}
		}

		public void Delete()
		{
			throw new System.NotImplementedException();
		}

		public void LoadFile(string filename)
		{
			this.CloseFile();
			this.resourceFile = this.resourceManager.EnsureFile(filename);
			this.resourceFile.Open();

			this.dataContext.Value = this.resourceFile.Items;
		}

		public void RecordCommand(string command)
		{
		}

		public void Redo()
		{
			
		}

		public void SaveFile(string filename)
		{
			//throw new NotImplementedException();
			//if (filename.EndsWith(".group.bin"))
			//{
			//    throw new NotImplementedException();
			//}
			//else
			//{
			//    using (var stream = new MemoryStream())
			//    {
			//        var w = new TextResourceWriter();
			//        w.Save(stream, (IEnumerable<Managed>)this.dataContext.Value);
			//    }
			//}
		}

		public void StopRecorder()
		{
		}

		public void Undo()
		{
			
		}

		public void onSelectAll()
		{
			
		}

		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.CloseFile();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.itemsPropertiesSplitter = new SplitContainer();
			var i = this.itemsPropertiesSplitter as ISupportInitialize;
			if (i != null)
			{
				i.BeginInit();
			}
			this.itemsPropertiesSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// itemsPropertiesSplitter
			// 
			this.itemsPropertiesSplitter.Dock = DockStyle.Fill;
			this.itemsPropertiesSplitter.Location = new Point(0, 0);
			this.itemsPropertiesSplitter.Name = "itemsPropertiesSplitter";
			this.itemsPropertiesSplitter.Size = new Size(150, 150);
			this.itemsPropertiesSplitter.TabIndex = 0;
			// 
			// ResourceFileEditor
			// 
			this.Controls.Add(this.itemsPropertiesSplitter);
			this.Name = "ResourceFileEditor";
			if (i != null)
			{
				i.EndInit();
			}
			this.itemsPropertiesSplitter.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		private void InitializeEditor()
		{
			var collectionView = new ListBox { Dock = DockStyle.Fill };
			collectionView.DisplayMember = "Resource";

			this.dataContext.DataContextChanged += (s, a) =>
				{
					collectionView.DataSource = this.dataContext.Value;
					var list = this.dataContext.Value as IList;
					if (list != null)
					{
						if (list.Count == 1)
						{
							this.itemsPropertiesSplitter.Panel1Collapsed = true;
						}
						else
						{
							this.itemsPropertiesSplitter.Panel1Collapsed = false;
						}
					}
				};

			collectionView.SelectedIndexChanged += (s, a) => this.ShowEditorFor((IResourceFileItem)collectionView.SelectedItem);

			//var collectionView = new CollectionView<Managed>(PreviewFor) { Dock = DockStyle.Fill };
			//new DataContextBinding(collectionView, dataContext, true);
			this.itemsPropertiesSplitter.Panel1.Controls.Add(collectionView);
			//itemsPropertiesSplitter.Panel2Collapsed = true;
		}

		private void ShowEditorFor(IResourceFileItem managed)
		{
			var controlCollection = this.itemsPropertiesSplitter.Panel2.Controls;
			if (managed == null)
			{
				return;
				//while (controlCollection.Count > 0)
				//{
				//    var c = controlCollection[0];
				//    controlCollection.RemoveAt(0);
				//    c.Dispose();
				//}
				//itemsPropertiesSplitter.Panel2Collapsed = true;
				//return;
			}

			if (controlCollection.Count > 0)
			{
				var existingView = (controlCollection[0] as IView);
				if (existingView != null)
				{
					var existingValue = existingView.DataContext.Value;
					if (existingValue != null)
					{
						if (managed.Resource.GetType() == existingValue.GetType())
						{
							existingView.DataContext.Value = managed.Resource;
							return;
						}
					}
				}
			}
			while (controlCollection.Count > 0)
			{
				var c = controlCollection[0];
				controlCollection.RemoveAt(0);
				c.Dispose();
			}

			var view = this.editorEnvironment.EditorFor(managed.Resource);
			view.DataContext.Value = managed.Resource;
			var v = view as Control;
			v.Dock = DockStyle.Fill;
			controlCollection.Add(v);

			this.itemsPropertiesSplitter.Panel2Collapsed = false;
		}

		#endregion
	}
}
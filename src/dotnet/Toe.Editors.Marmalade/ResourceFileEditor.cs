using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Resources;
using Toe.Utils.Mesh.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class ResourceFileEditor : System.Windows.Forms.UserControl, IResourceEditor
	{
		private readonly IEditorEnvironment editorEnvironment;

		private readonly IResourceManager resourceManager;

		private SplitContainer itemsPropertiesSplitter;

		private DataContextContainer dataContext = new DataContextContainer();

		private IResourceFile resourceFile;

		public ResourceFileEditor(IEditorEnvironment editorEnvironment, IResourceManager resourceManager)
		{
			this.editorEnvironment = editorEnvironment;
			this.resourceManager = resourceManager;
			this.Dock = DockStyle.Fill;

			this.InitializeComponent();
			this.InitializeEditor();
		}

		private void InitializeEditor()
		{
			var collectionView = new ListBox() { Dock = DockStyle.Fill };
			collectionView.DisplayMember = "Resource";

			this.dataContext.DataContextChanged += (s, a) =>
				{
					collectionView.DataSource = this.dataContext.Value;
					var list = this.dataContext.Value as IList;
					if (list != null)
					{
						if (list.Count == 1)
							itemsPropertiesSplitter.Panel1Collapsed = true;
						else
							itemsPropertiesSplitter.Panel1Collapsed = false;
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
			var controlCollection = itemsPropertiesSplitter.Panel2.Controls;
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
						if (managed.Resource.GetType() == existingValue.GetType())
						{
							existingView.DataContext.Value = managed.Resource;
							return;
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
			controlCollection.Add((Control)v);

			itemsPropertiesSplitter.Panel2Collapsed = false;
		}

		#region Implementation of IResourceEditor

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
				return resourceFile.FilePath;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				CloseFile();
			}
			base.Dispose(disposing);
		}

		public void CloseFile()
		{
			if (this.resourceFile != null)
			{
				this.resourceFile.Close();
				this.dataContext.Value = null;
				this.resourceFile = null;
			}
		}
		public void LoadFile(string filename)
		{
			CloseFile();
			this.resourceFile = resourceManager.EnsureFile(filename);
			this.resourceFile.Open();

			this.dataContext.Value = this.resourceFile.Items;
		}

		

		public void RecordCommand(string command)
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

		#endregion

		private void InitializeComponent()
		{
			this.itemsPropertiesSplitter = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.itemsPropertiesSplitter)).BeginInit();
			this.itemsPropertiesSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// itemsPropertiesSplitter
			// 
			this.itemsPropertiesSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.itemsPropertiesSplitter.Location = new System.Drawing.Point(0, 0);
			this.itemsPropertiesSplitter.Name = "itemsPropertiesSplitter";
			this.itemsPropertiesSplitter.Size = new System.Drawing.Size(150, 150);
			this.itemsPropertiesSplitter.TabIndex = 0;
			// 
			// ResourceFileEditor
			// 
			this.Controls.Add(this.itemsPropertiesSplitter);
			this.Name = "ResourceFileEditor";
			((System.ComponentModel.ISupportInitialize)(this.itemsPropertiesSplitter)).EndInit();
			this.itemsPropertiesSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
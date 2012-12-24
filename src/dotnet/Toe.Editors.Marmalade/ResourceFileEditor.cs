using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Utils.Mesh.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwGx;

namespace Toe.Editors.Marmalade
{
	public class ResourceFileEditor : System.Windows.Forms.UserControl, IResourceEditor
	{
		private DataContextContainer dataContext = new DataContextContainer();

		public ResourceFileEditor()
		{
			this.Dock = DockStyle.Fill;
			var collectionView = new CollectionView<Managed>(OnViewFabric) {Dock = DockStyle.Fill};
			new DataContextBinding(collectionView, dataContext, true);
			this.Controls.Add(collectionView);
		}

		private IView OnViewFabric(Managed m)
		{
			if (m is Material) return new MaterialEditor();
			return new StringView();
		}

		#region Implementation of IResourceEditor

		public Control Control
		{
			get
			{
				return this;
			}
		}

		public void LoadFile(string filename)
		{
			if (filename.EndsWith(".group.bin"))
			{

			}
			else
			{
				using (var fileStream = File.OpenRead(filename))
				{
					var r = new TextResourceReader();
					this.dataContext.Value = r.Load(fileStream, Path.GetDirectoryName(Path.GetFullPath(filename)));
				}
			}
		}

		public void RecordCommand(string command)
		{
		}

		public void SaveFile(string filename)
		{
			if (filename.EndsWith(".group.bin"))
			{
				throw new NotImplementedException();
			}
			else
			{
				using (var stream = new MemoryStream())
				{
					var w = new TextResourceWriter();
					w.Save(stream, (IEnumerable<Managed>)this.dataContext.Value);
				}
			}
		}

		public void StopRecorder()
		{
		}

		#endregion
	}
}
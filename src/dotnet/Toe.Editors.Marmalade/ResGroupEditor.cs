using System.Collections.Generic;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Interfaces.Views;
using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwResManager;

namespace Toe.Editors.Marmalade
{
	public class ResGroupEditor : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		private readonly ICommandHistory history;

		#endregion

		#region Constructors and Destructors

		public ResGroupEditor(IEditorEnvironment editorEnvironment, ICommandHistory history)
		{
			this.editorEnvironment = editorEnvironment;
			this.history = history;
			this.AutoSize = true;
			this.Padding = new Padding(10);

			var resourceGroup = new GroupBox
				{ Text = "Resource Group", Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(10) };
			this.Controls.Add(resourceGroup);

			var sp = new StackPanel() { Dock = DockStyle.Fill, AutoSize = true };
			resourceGroup.Controls.Add(sp);

			var collectionView = new CollectionView<IResourceFile>(a => editorEnvironment.EditorFor(a,history)) { Dock = DockStyle.Fill };
			new PropertyBinding<ResGroup, IList<IResourceFile>>(collectionView, this.dataContext, m => m.ExternalResources, null);
			sp.Controls.Add(collectionView);

			var embCollectionView = new CollectionView<IResourceFile>(a => editorEnvironment.EditorFor(a, history)) { Dock = DockStyle.Fill };
			new PropertyBinding<ResGroup, IList<Managed>>(embCollectionView, this.dataContext, m => m.EmbeddedResources, null);
			sp.Controls.Add(embCollectionView);
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public ResGroup Material
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (ResGroup)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion
	}
}
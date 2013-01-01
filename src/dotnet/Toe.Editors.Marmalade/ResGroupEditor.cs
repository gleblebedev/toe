using System.Collections.Generic;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Resources;
using Toe.Utils.Marmalade.IwResManager;

namespace Toe.Editors.Marmalade
{
	public class ResGroupEditor : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly IEditorEnvironment editorEnvironment;

		#endregion

		#region Constructors and Destructors

		public ResGroupEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			this.AutoSize = true;
			this.Padding = new Padding(10);

			var resourceGroup = new GroupBox
				{ Text = "Resource Group", Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(10) };
			this.Controls.Add(resourceGroup);

			var collectionView = new CollectionView<IResourceFile>(a => editorEnvironment.EditorFor(a)) { Dock = DockStyle.Fill };
			new PropertyBinding<ResGroup, IList<IResourceFile>>(collectionView, this.dataContext, m => m.ExternalResources, null);
			resourceGroup.Controls.Add(collectionView);
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
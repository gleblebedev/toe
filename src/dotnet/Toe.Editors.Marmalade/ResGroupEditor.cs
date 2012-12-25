using System.Collections.Generic;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Utils.Mesh.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Editors.Marmalade
{
	public class ResGroupEditor : System.Windows.Forms.UserControl, IView
	{
		private readonly IEditorEnvironment editorEnvironment;

		private DataContextContainer dataContext = new DataContextContainer();

		public ResGroupEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			this.AutoSize = true;
			this.Padding = new Padding(10);

			var resourceGroup = new GroupBox { Text = "Resource Group", Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(10) };
			this.Controls.Add(resourceGroup);

			var collectionView = new CollectionView<Managed>(a=>editorEnvironment.EditorFor(a)) { Dock = DockStyle.Fill };
			new PropertyBinding<ResGroup,IList<Managed>>(collectionView, dataContext, m=>m.EmbeddedResources, null);
		}

		#region Implementation of IView

		public ResGroup Material
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return (ResGroup)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		#endregion
	}
}
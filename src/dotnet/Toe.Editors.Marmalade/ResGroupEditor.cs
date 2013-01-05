using System.Collections.Generic;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;
using Toe.Editors.Interfaces.Views;
using Toe.Marmalade;
using Toe.Marmalade.IwResManager;
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

		private SplitContainer split;

		#endregion

		#region Constructors and Destructors

		public ResGroupEditor(IEditorEnvironment editorEnvironment, ICommandHistory history)
		{
			this.editorEnvironment = editorEnvironment;
			this.history = history;
			this.AutoSize = true;
			this.Padding = new Padding(10);

			this.SuspendLayout();

			split = new SplitContainer() { Dock = DockStyle.Fill };
			split.Panel2Collapsed = true;
			this.Controls.Add(split);

			var sp = new StackPanel() { Dock = DockStyle.Fill, AutoSize = true };
			split.Panel1.Controls.Add(sp);

			var collectionView = new CollectionView<IResourceFile>(a => editorEnvironment.EditorFor(a, history)) { AutoSize = true };
			collectionView.ItemsPanel.AutoSize = true;
			collectionView.ItemsPanel.AutoScroll = false;
			new PropertyBinding<ResGroup, IList<IResourceFile>>(collectionView, this.dataContext, m => m.ExternalResources, null);
			sp.Controls.Add(collectionView);

			var embCollectionView = new CollectionView<Managed>(a => CreateButtonForResource(a)) { AutoSize = true};
			embCollectionView.ItemsPanel.AutoSize = true;
			embCollectionView.ItemsPanel.AutoScroll = false;
			new PropertyBinding<ResGroup, IList<Managed>>(embCollectionView, this.dataContext, m => m.EmbeddedResources, null);
			sp.Controls.Add(embCollectionView);

			this.ResumeLayout();
			this.PerformLayout();
		}

		private IView CreateButtonForResource(Managed managed)
		{
			var buttonForResource = new ButtonView();
			buttonForResource.Click += (s, a) => OpenEditorForResource(managed);
			return buttonForResource;
		}

		private void OpenEditorForResource(Managed managed)
		{
			split.Panel2Collapsed = false;
			split.Panel2.Controls.Clear();
			IView editorFor = editorEnvironment.EditorFor(managed, history);
			editorFor.DataContext.Value = managed;
			var control = (Control)editorFor;
			control.Dock = DockStyle.Fill;
			split.Panel2.Controls.Add(control);
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
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;

namespace Toe.Editors.Interfaces.Views
{
	public class CollectionView<T> : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly Func<T, IView> viewFabric;

		private readonly List<ItemContext> views = new List<ItemContext>();

		private Panel stackPanel;

		#endregion

		#region Constructors and Destructors

		public CollectionView(Func<T, IView> viewFabric)
		{
			this.ItemsPanel = new StackPanel { Dock = DockStyle.Fill };
			this.Controls.Add(this.ItemsPanel);
			this.viewFabric = viewFabric;
			this.dataContext.DataContextChanged += this.OnDataContextChanged;
			this.dataContext.CollectionChanged += this.OnItemsCollectionChanged;
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

		public IEnumerable<T> Items
		{
			get
			{
				return this.dataContext.Value as IEnumerable<T>;
			}
		}

		public Panel ItemsPanel
		{
			get
			{
				return this.stackPanel;
			}
			set
			{
				if (this.stackPanel != value)
				{
					if (this.stackPanel != null)
					{
						this.Controls.Remove(this.stackPanel);
					}
					this.stackPanel = value;
					if (this.stackPanel != null)
					{
						this.Controls.Add(this.stackPanel);
					}
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public override Size GetPreferredSize(Size proposedSize)
		{
			return this.ItemsPanel.GetPreferredSize(proposedSize);
		}

		#endregion

		#region Methods

		private void ClearItemsCollection()
		{
			this.views.Clear();
			this.ItemsPanel.Controls.Clear();
		}

		private void OnDataContextChanged(object sender, DataContextChangedEventArgs e)
		{
			this.ResetItemsCollection();
		}

		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.ResetItemsCollection();
		}

		private void ResetItemsCollection()
		{
			this.ClearItemsCollection();
			var e = this.DataContext.Value as IEnumerable<T>;
			if (e != null)
			{
				foreach (var item in this.Items)
				{
					var view = this.viewFabric(item);
					this.views.Add(new ItemContext { View = view });
					view.DataContext.Value = item;
					this.ItemsPanel.Controls.Add((Control)view);
				}
			}
		}

		#endregion

		internal class ItemContext
		{
			#region Public Properties

			public IView View { get; set; }

			#endregion
		}
	}
}
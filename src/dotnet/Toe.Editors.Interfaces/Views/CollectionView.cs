using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Panels;

namespace Toe.Editors.Interfaces.Views
{
	public class CollectionView<T>: UserControl, IView
	{
		private readonly Func<T, IView> viewFabric;

		internal class ItemContext
		{
			public IView View { get; set; }
		}
		List<ItemContext> views = new List<ItemContext>();

		public CollectionView(Func<T,IView> viewFabric)
		{
			this.ItemsPanel = new StackPanel() { Dock = DockStyle.Fill };
			this.Controls.Add(this.ItemsPanel);
			this.viewFabric = viewFabric;
			dataContext.DataContextChanged += this.OnDataContextChanged;
			dataContext.CollectionChanged += OnItemsCollectionChanged;
		}
		public override Size GetPreferredSize(Size proposedSize)
		{
			return this.ItemsPanel.GetPreferredSize(proposedSize);
		}
		private void OnDataContextChanged(object sender, DataContextChangedEventArgs e)
		{
			ResetItemsCollection();
		}

		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ResetItemsCollection();
		}
		private void ClearItemsCollection()
		{
			views.Clear();
			this.ItemsPanel.Controls.Clear();
		}

		private void ResetItemsCollection()
		{
			ClearItemsCollection();
			var e = DataContext.Value as IEnumerable<T>;
			if (e != null)
			{
				foreach (var item in Items)
				{
					var view = viewFabric(item);
					views.Add(new ItemContext(){View = view});
					view.DataContext.Value = item;
					this.ItemsPanel.Controls.Add((Control)view);
				}
			}
		}

		DataContextContainer dataContext = new DataContextContainer();

		private Panel stackPanel;

		#region Implementation of IView
		public IEnumerable<T> Items
		{
			get
			{
				return this.dataContext.Value as IEnumerable<T>;
			}
		}
		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
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
						this.Controls.Remove(this.stackPanel);
					this.stackPanel = value;
					if (this.stackPanel != null)
						this.Controls.Add(this.stackPanel);
				}
			}
		}

		#endregion
	}
}
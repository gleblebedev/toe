using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

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
			stackPanel = new StackPanel() { 
				Dock = DockStyle.Fill
			                              };
			
			this.Controls.Add(stackPanel);
			this.viewFabric = viewFabric;
			dataContext.DataContextChanged += this.OnDataContextChanged;
			dataContext.CollectionChanged += OnItemsCollectionChanged;
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
			stackPanel.Controls.Clear();
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
					stackPanel.Controls.Add((Control)view);
				}
			}
		}

		DataContextContainer dataContext = new DataContextContainer();

		private StackPanel stackPanel;

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

		#endregion
	}
}
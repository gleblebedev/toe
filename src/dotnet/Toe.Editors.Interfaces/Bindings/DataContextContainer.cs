using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Toe.Editors.Interfaces.Bindings
{
	public class DataContextContainer : INotifyPropertyChanging, INotifyPropertyChanged, INotifyCollectionChanged
	{
		private object value;

		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (!Equals(this.value, value))
				{
					Unsubscribe();
					var oldValue = this.value;
					this.value = value;
					if (DataContextChanged != null) DataContextChanged(this, new DataContextChangedEventArgs(oldValue, value));
					Subscribe();
				}
			}
		}

		private void Subscribe()
		{
			if (this.value == null)
				return;

			var notifyCollectionChanged = this.value as INotifyCollectionChanged;
			if (notifyCollectionChanged != null) notifyCollectionChanged.CollectionChanged += OnDataCollectionChanged;

			var bindingList = this.value as IBindingList;
			if (bindingList != null) bindingList.ListChanged += OnBindingListChanged;
			

			var notifyPropertyChanged = this.value as INotifyPropertyChanged;
			if (notifyPropertyChanged != null) notifyPropertyChanged.PropertyChanged += OnDataPropertyChanged;

			var notifyPropertyChanging = this.value as INotifyPropertyChanging;
			if (notifyPropertyChanging != null) notifyPropertyChanging.PropertyChanging += OnDataPropertyChanging;
		}

		private void OnBindingListChanged(object sender, ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.Reset:
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
					break;
				case ListChangedType.ItemAdded:
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, ((IList)sender)[e.NewIndex], e.NewIndex));
					break;
				case ListChangedType.ItemDeleted:
					//TODO: implement conversion
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
					break;
				case ListChangedType.ItemMoved:
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, ((IList)sender)[e.NewIndex], e.NewIndex, e.OldIndex));
					break;
				case ListChangedType.ItemChanged:
					//TODO: implement conversion
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
					break;
				case ListChangedType.PropertyDescriptorAdded:
					break;
				case ListChangedType.PropertyDescriptorDeleted:
					break;
				case ListChangedType.PropertyDescriptorChanged:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void Unsubscribe()
		{
			if (this.value == null)
				return;

			var notifyCollectionChanged = this.value as INotifyCollectionChanged;
			if (notifyCollectionChanged != null) notifyCollectionChanged.CollectionChanged -= OnDataCollectionChanged;

			var notifyPropertyChanged = this.value as INotifyPropertyChanged;
			if (notifyPropertyChanged != null) notifyPropertyChanged.PropertyChanged -= OnDataPropertyChanged;

			var notifyPropertyChanging = this.value as INotifyPropertyChanging;
			if (notifyPropertyChanging != null) notifyPropertyChanging.PropertyChanging -= OnDataPropertyChanging;
		}

		private void OnDataPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			this.RaiseDataPropertyChanging(e.PropertyName);
		}

		private void OnDataPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RaiseDataPropertyChanged(e.PropertyName);
		}

		private void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.RaiseDataCollectionChanged(e);
		}

		protected virtual void RaiseDataPropertyChanging(string propertyName)
		{
			if (PropertyChanging != null)
				PropertyChanging(this.value, new PropertyChangingEventArgs(propertyName));
		}
		protected virtual void RaiseDataPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this.value, new PropertyChangedEventArgs(propertyName));
		}
		protected virtual void RaiseDataCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (CollectionChanged != null)
				CollectionChanged(this.value, args);
		}
		#region Implementation of INotifyPropertyChanging

		public event EventHandler<DataContextChangedEventArgs> DataContextChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Implementation of INotifyCollectionChanged

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion
	}
}

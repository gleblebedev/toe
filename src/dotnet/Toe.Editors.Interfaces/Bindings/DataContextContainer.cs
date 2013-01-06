using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Toe.Editors.Interfaces.Bindings
{
	public class DataContextContainer : INotifyPropertyChanging, INotifyPropertyChanged, INotifyCollectionChanged
	{
		#region Constants and Fields

		private object value;

		#endregion

		#region Public Events

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public event EventHandler<DataContextChangedEventArgs> DataContextChanged;

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region Public Properties

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
					this.Unsubscribe();
					var oldValue = this.value;
					this.value = value;
					if (this.DataContextChanged != null)
					{
						this.DataContextChanged(this, new DataContextChangedEventArgs(oldValue, value));
					}
					this.Subscribe();
				}
			}
		}

		#endregion

		#region Methods

		protected virtual void RaiseDataCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this.value, args);
			}
		}

		protected virtual void RaiseDataPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this.value, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual void RaiseDataPropertyChanging(string propertyName)
		{
			if (this.PropertyChanging != null)
			{
				this.PropertyChanging(this.value, new PropertyChangingEventArgs(propertyName));
			}
		}

		private void OnBindingListChanged(object sender, ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.Reset:
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
					break;
				case ListChangedType.ItemAdded:
					this.RaiseDataCollectionChanged(
						new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, ((IList)sender)[e.NewIndex], e.NewIndex));
					break;
				case ListChangedType.ItemDeleted:
					//TODO: implement conversion
					this.RaiseDataCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
					break;
				case ListChangedType.ItemMoved:
					this.RaiseDataCollectionChanged(
						new NotifyCollectionChangedEventArgs(
							NotifyCollectionChangedAction.Move, ((IList)sender)[e.NewIndex], e.NewIndex, e.OldIndex));
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

		private void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.RaiseDataCollectionChanged(e);
		}

		private void OnDataPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RaiseDataPropertyChanged(e.PropertyName);
		}

		private void OnDataPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			this.RaiseDataPropertyChanging(e.PropertyName);
		}

		private void Subscribe()
		{
			if (this.value == null)
			{
				return;
			}

			var notifyCollectionChanged = this.value as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged += this.OnDataCollectionChanged;
			}

			var bindingList = this.value as IBindingList;
			if (bindingList != null)
			{
				bindingList.ListChanged += this.OnBindingListChanged;
			}

			var notifyPropertyChanged = this.value as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += this.OnDataPropertyChanged;
			}

			var notifyPropertyChanging = this.value as INotifyPropertyChanging;
			if (notifyPropertyChanging != null)
			{
				notifyPropertyChanging.PropertyChanging += this.OnDataPropertyChanging;
			}
		}

		private void Unsubscribe()
		{
			if (this.value == null)
			{
				return;
			}

			var notifyCollectionChanged = this.value as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= this.OnDataCollectionChanged;
			}

			var notifyPropertyChanged = this.value as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.OnDataPropertyChanged;
			}

			var notifyPropertyChanging = this.value as INotifyPropertyChanging;
			if (notifyPropertyChanging != null)
			{
				notifyPropertyChanging.PropertyChanging -= this.OnDataPropertyChanging;
			}
		}

		#endregion
	}
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Toe.Utils;

namespace Toe.Core
{
	public class ToeSystemConfiguration : INotifyPropertyChanged
	{
		private string systemName;
		protected uint systemId;

		public ToeSystemConfiguration()
		{
			this.layers.CollectionChanged += this.OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.RaisePropertyChanged("Layers");
		}

		private readonly ObservableCollection<ToeLayerConfiguration> layers = new ObservableCollection<ToeLayerConfiguration>();

		public IList<ToeLayerConfiguration> Layers
		{
			get
			{
				return this.layers;
			}
		}

		public string SystemName
		{
			get
			{
				return this.systemName;
			}
			set
			{
				if (this.systemName != value)
				{
					this.systemName = value;
					this.systemId = Hash.Get(value);
					this.RaisePropertyChanged("SystemName");
					this.RaisePropertyChanged("SystemId");
				}
			}
		}
		public uint SystemID
		{
			get
			{
				return this.systemId;
			}
			set
			{
				if (this.systemId != value)
				{
					this.systemName = null;
					this.systemId = value;
					this.RaisePropertyChanged("SystemName");
					this.RaisePropertyChanged("SystemId");
				}
			}
		}
		protected virtual void RaisePropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
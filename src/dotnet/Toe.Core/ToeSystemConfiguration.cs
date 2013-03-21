using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Toe.Utils;

namespace Toe.Core
{
	public class ToeSystemConfiguration : INotifyPropertyChanged
	{
		#region Constants and Fields

		protected uint systemId;

		private readonly ObservableCollection<ToeLayerConfiguration> layers =
			new ObservableCollection<ToeLayerConfiguration>();

		private string systemName;

		#endregion

		#region Constructors and Destructors

		public ToeSystemConfiguration()
		{
			this.layers.CollectionChanged += this.OnCollectionChanged;
		}

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

		public IList<ToeLayerConfiguration> Layers
		{
			get
			{
				return this.layers;
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

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.RaisePropertyChanged("Layers");
		}

		#endregion
	}
}
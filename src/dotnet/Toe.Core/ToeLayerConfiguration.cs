using System.ComponentModel;

using Toe.Utils;

namespace Toe.Core
{
	public class ToeLayerConfiguration: INotifyPropertyChanged
	{
		private string layerName;
		protected uint LayerId;

		private ToeComponentLayerPopularity popularity = ToeComponentLayerPopularity.Average;

		public ToeComponentLayerPopularity Popularity
		{
			get
			{
				return this.popularity;
			}
			set
			{
				if (this.popularity != value)
				{
					this.popularity = value;
					this.RaisePropertyChanged("Popularity");
				}
			}
		}
		public string LayerName
		{
			get
			{
				return this.layerName;
			}
			set
			{
				if (this.layerName != value)
				{
					this.layerName = value;
					this.LayerId = Hash.Get(value);
					this.RaisePropertyChanged("LayerName");
					this.RaisePropertyChanged("LayerId");
				}
			}
		}
		public uint LayerID
		{
			get
			{
				return this.LayerId;
			}
			set
			{
				if (this.LayerId != value)
				{
					this.layerName = null;
					this.LayerId = value;
					this.RaisePropertyChanged("LayerName");
					this.RaisePropertyChanged("LayerId");
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
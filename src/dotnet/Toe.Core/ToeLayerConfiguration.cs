using System.ComponentModel;

using Toe.Utils;

namespace Toe.Core
{
	public class ToeLayerConfiguration : INotifyPropertyChanged
	{
		#region Constants and Fields

		protected uint LayerId;

		private string layerName;

		private ToeComponentLayerPopularity popularity = ToeComponentLayerPopularity.Average;

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

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

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		#endregion
	}
}
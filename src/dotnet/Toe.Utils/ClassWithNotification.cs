using System.ComponentModel;

namespace Toe.Utils
{
	public class ClassWithNotification : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(PropertyChangedEventArgs property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, property);
			}
		}

		protected virtual void RaisePropertyChanging(PropertyChangingEventArgs property)
		{
			if (this.PropertyChanging != null)
			{
				this.PropertyChanging(this, property);
			}
		}

		#endregion
	}
}
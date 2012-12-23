using System.ComponentModel;

namespace Toe.Utils.Mesh.Marmalade
{
	public class Managed: INotifyPropertyChanged
	{
		private string name;

		/// <summary>
		/// Object name.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.RaisePropertyChanged("Name");
				}
			}
		}

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
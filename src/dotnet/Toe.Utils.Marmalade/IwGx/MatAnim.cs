using System.ComponentModel;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class MatAnim : INotifyPropertyChanged
	{
		public byte CelH { get; set; }

		public byte CelW { get; set; }

		public byte CelNum { get; set; }

		public byte CelNumU { get; set; }

		public byte CelPeriod { get; set; }

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
using System.ComponentModel;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class MatAnim : INotifyPropertyChanged
	{
		public byte CelH { get; set; }

		public byte CelW { get; set; }

		public byte CelNum { get; set; }

		private byte celNumU = 1;

		public byte CelNumU
		{
			get
			{
				return this.celNumU;
			}
			set
			{
				this.celNumU = value;
			}
		}

		private byte celPeriod = 1;

		public byte CelPeriod
		{
			get
			{
				return this.celPeriod;
			}
			set
			{
				this.celPeriod = value;
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
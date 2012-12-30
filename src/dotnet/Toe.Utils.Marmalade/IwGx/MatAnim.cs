using System.ComponentModel;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class MatAnim : INotifyPropertyChanged
	{
		#region Constants and Fields

		private byte celNumU = 1;

		private byte celPeriod = 1;

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

		public byte CelH { get; set; }

		public byte CelNum { get; set; }

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

		public byte CelW { get; set; }

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}
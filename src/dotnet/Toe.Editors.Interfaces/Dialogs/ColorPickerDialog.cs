using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;

namespace Toe.Editors.Interfaces.Dialogs
{
	public partial class ColorPickerDialog : Form
	{
		public class RGBA:INotifyPropertyChanged
		{
			public RGBA(Color c)
			{
				r = c.R;
				g = c.G;
				b = c.B;
				a = c.A;
			}
			private byte r;

			public byte R
			{
				get
				{
					return this.r;
				}
				set
				{
					if (this.r != value)
					{
						this.r = value;
						this.RaisePropertyChanged("R");
					}
				}
			}
			private byte g;

			public byte G
			{
				get
				{
					return this.g;
				}
				set
				{
					if (this.g != value)
					{
						this.g = value;
						this.RaisePropertyChanged("G");
					}
				}
			}
			private byte b;

			public byte B
			{
				get
				{
					return this.b;
				}
				set
				{
					if (this.b != value)
					{
						this.b = value;
						this.RaisePropertyChanged("B");
					}
				}
			}
			private byte a;

			public byte A
			{
				get
				{
					return this.a;
				}
				set
				{
					if (this.a != value)
					{
						this.a = value;
						this.RaisePropertyChanged("A");
					}
				}
			}

			public Color Color { get
			{
				return Color.FromArgb(a, r, g, b);
			} }

			PropertyChangedEventArgs colorChangedEventArgs = new PropertyChangedEventArgs("Color");

			protected void RaisePropertyChanged(string property)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(property));
					PropertyChanged(this, colorChangedEventArgs);
				}
			}

			#region Implementation of INotifyPropertyChanged

			public event PropertyChangedEventHandler PropertyChanged;

			#endregion
		}
		private DataContextContainer dataContext;
		public ColorPickerDialog(Color c)
		{
			this.InitializeComponent();
			originalColor.DataContext.Value = c;
			dataContext = new DataContextContainer();
			new PropertyBinding<RGBA, Color>(newColor, dataContext, (m)=>m.Color,null);
			new PropertyBinding<RGBA, byte>(editR,dataContext,(m)=>m.R,(m,v)=>m.R=v);
			new PropertyBinding<RGBA, byte>(editG, dataContext, (m) => m.G, (m, v) => m.G = v);
			new PropertyBinding<RGBA, byte>(editB, dataContext, (m) => m.B, (m, v) => m.B = v);
			new PropertyBinding<RGBA, byte>(editA, dataContext, (m) => m.A, (m, v) => m.A = v);
			dataContext.Value = new RGBA(c);
		}

		public Color SelectedColor
		{
			get
			{
				return ((RGBA)dataContext.Value).Color;
			}
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

	}
}

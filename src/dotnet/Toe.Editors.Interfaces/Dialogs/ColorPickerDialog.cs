using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Dialogs
{
	public partial class ColorPickerDialog : Form
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext;

		#endregion

		#region Constructors and Destructors

		public ColorPickerDialog(Color c)
		{
			this.InitializeComponent();
			this.originalColor.DataContext.Value = c;
			this.dataContext = new DataContextContainer();
			new PropertyBinding<RGBA, Color>(this.newColor, this.dataContext, (m) => m.Color, null);
			new PropertyBinding<RGBA, byte>(this.editR, this.dataContext, (m) => m.R, (m, v) => m.R = v);
			new PropertyBinding<RGBA, byte>(this.editG, this.dataContext, (m) => m.G, (m, v) => m.G = v);
			new PropertyBinding<RGBA, byte>(this.editB, this.dataContext, (m) => m.B, (m, v) => m.B = v);
			new PropertyBinding<RGBA, byte>(this.editA, this.dataContext, (m) => m.A, (m, v) => m.A = v);
			this.dataContext.Value = new RGBA(c);
		}

		#endregion

		#region Public Properties

		public Color SelectedColor
		{
			get
			{
				return ((RGBA)this.dataContext.Value).Color;
			}
		}

		#endregion

		#region Methods

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		#endregion

		public class RGBA : INotifyPropertyChanged
		{
			#region Constants and Fields

			private readonly PropertyChangedEventArgs colorChangedEventArgs = new PropertyChangedEventArgs("Color");

			private byte a;

			private byte b;

			private byte g;

			private byte r;

			#endregion

			#region Constructors and Destructors

			public RGBA(Color c)
			{
				this.r = c.R;
				this.g = c.G;
				this.b = c.B;
				this.a = c.A;
			}

			#endregion

			#region Public Events

			public event PropertyChangedEventHandler PropertyChanged;

			#endregion

			#region Public Properties

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

			public Color Color
			{
				get
				{
					return Color.FromArgb(this.a, this.r, this.g, this.b);
				}
			}

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

			#endregion

			#region Methods

			protected void RaisePropertyChanged(string property)
			{
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs(property));
					this.PropertyChanged(this, this.colorChangedEventArgs);
				}
			}

			#endregion
		}
	}
}
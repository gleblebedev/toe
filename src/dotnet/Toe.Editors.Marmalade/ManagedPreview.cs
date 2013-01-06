using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Marmalade;

namespace Toe.Editors.Marmalade
{
	public class ManagedPreview : UserControl, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		#endregion

		#region Constructors and Destructors

		public ManagedPreview()
		{
			this.AutoSize = true;

			var resourceGroup = new GroupBox { Text = "fff", Dock = DockStyle.Fill, AutoSize = true, Height = 64 + 30 };
			var pictureBox = new PictureBox { Image = Resources.material, Width = 64, Height = 64, Left = 10, Top = 20 };
			resourceGroup.Controls.Add(pictureBox);
			var nameView = new StringView { Left = 10 + 64 + 10, Top = 20, Font = new Font(FontFamily.GenericSerif, 14) };
			resourceGroup.Controls.Add(nameView);
			new PropertyBinding<Managed, string>(nameView, this.dataContext, a => a.Name, null);
			this.dataContext.DataContextChanged +=
				(s, a) => { resourceGroup.Text = (this.dataContext.Value == null) ? "" : this.dataContext.Value.GetType().Name; };

			this.Controls.Add(resourceGroup);
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public Managed Managed
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return (Managed)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion
	}
}
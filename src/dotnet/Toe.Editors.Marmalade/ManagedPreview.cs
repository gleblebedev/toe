using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Utils.Mesh.Marmalade;
using Toe.Utils.Mesh.Marmalade.IwResManager;

namespace Toe.Editors.Marmalade
{
	public class ManagedPreview : System.Windows.Forms.UserControl, IView
	{
		private DataContextContainer dataContext = new DataContextContainer();

		public ManagedPreview()
		{
			this.AutoSize = true;

			var resourceGroup = new GroupBox { Text = "fff",Dock = DockStyle.Fill,  AutoSize = true,Height = 64+30};
			var pictureBox = new PictureBox { Image = Resources.material, Width = 64, Height = 64, Left = 10, Top = 20};
			resourceGroup.Controls.Add(pictureBox);
			var nameView = new StringView{Left = 10+64+10, Top = 20, Font = new Font(FontFamily.GenericSerif, 14)};
			resourceGroup.Controls.Add(nameView);
			new PropertyBinding<Managed, string>(nameView, dataContext, a => a.Name, null);
			dataContext.DataContextChanged +=
				(s, a) => { resourceGroup.Text = (dataContext.Value == null) ? "" : dataContext.Value.GetType().Name; };

			this.Controls.Add(resourceGroup);
		}

		#region Implementation of IView

		public Managed Managed
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return (Managed)this.dataContext.Value;
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		#endregion
	}
}
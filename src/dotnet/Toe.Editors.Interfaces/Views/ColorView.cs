using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class ColorView : UserControl, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		public ColorView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			if (this.dataContext.Value != null)
			{
				this.BackColor = (Color)this.dataContext.Value;
			}
		}


		#region Implementation of IView

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
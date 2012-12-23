using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class StringView: UserControl, IView
	{
		DataContextContainer dataContext = new DataContextContainer();

		private Label label;

		public StringView()
		{
			label = new Label();
			this.Controls.Add(label);
			dataContext.DataContextChanged += UpdateLabel;
		}

		private void UpdateLabel(object sender, DataContextChangedEventArgs e)
		{
			label.Text = string.Format(CultureInfo.InvariantCulture, "{0}", e.NewValue);
		}

		#region Implementation of IView

		public DataContextContainer DataContext
		{
			get
			{
				return dataContext;
			}
		}

		#endregion
	}
}

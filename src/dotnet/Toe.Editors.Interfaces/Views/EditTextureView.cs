using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditTextureView : SingleControlView<ButtonView>, IView
	{
		DataContextContainer dataContext = new DataContextContainer();

		public EditTextureView()
		{
			//this.label.AutoSize = true;
			new DataContextBinding(ViewControl, this.DataContext,false);
		}

		#region Implementation of IView

		public string Text
		{
			get
			{
				if (this.dataContext.Value == null) return null;
				return this.dataContext.Value.ToString();
			}
			set
			{
				this.dataContext.Value  = value;
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
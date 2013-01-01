using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditShaderView : SingleControlView<ButtonView>, IView
	{
		private readonly ICommandHistory history;

		DataContextContainer dataContext = new DataContextContainer();

		public EditShaderView(ICommandHistory history)
		{
			this.history = history;
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
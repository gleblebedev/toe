using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditShaderView : SingleControlView<ButtonView>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly ICommandHistory history;

		#endregion

		#region Constructors and Destructors

		public EditShaderView(ICommandHistory history)
		{
			this.history = history;
			//this.label.AutoSize = true;
			new DataContextBinding(this.ViewControl, this.DataContext, false);
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

		public new string Text
		{
			get
			{
				if (this.dataContext.Value == null)
				{
					return null;
				}
				return this.dataContext.Value.ToString();
			}
			set
			{
				this.dataContext.Value = value;
			}
		}

		#endregion
	}
}
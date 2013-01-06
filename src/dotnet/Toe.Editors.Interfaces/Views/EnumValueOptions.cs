namespace Toe.Editors.Interfaces.Views
{
	public class EnumValueOptions
	{
		#region Constructors and Destructors

		public EnumValueOptions(string text)
		{
			this.Text = text;
		}

		#endregion

		#region Public Properties

		public bool IsSelectable { get; set; }

		public string Text { get; set; }

		#endregion
	}
}
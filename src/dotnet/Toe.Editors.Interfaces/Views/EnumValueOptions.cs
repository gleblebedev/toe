namespace Toe.Editors.Interfaces.Views
{
	public class EnumValueOptions
	{
		public EnumValueOptions(string text)
		{
			this.Text = text;
		}

		public string Text { get; set; }

		public bool IsSelectable { get; set; }
	}
}
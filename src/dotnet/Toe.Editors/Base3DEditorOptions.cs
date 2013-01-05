namespace Toe.Editors
{
	public class Base3DEditorOptions
	{
		private bool lighting = true;

		public bool Lighting
		{
			get
			{
				return this.lighting;
			}
			set
			{
				this.lighting = value;
			}
		}
	}
}
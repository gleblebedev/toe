namespace Toe.Editors
{
	public class Base3DEditorOptions
	{
		#region Constants and Fields

		private bool lighting = true;

		#endregion

		#region Public Properties

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

		#endregion
	}
}
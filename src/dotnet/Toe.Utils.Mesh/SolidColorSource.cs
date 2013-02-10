namespace Toe.Utils.Mesh
{
	public class SolidColorSource : IColorSource
	{
		#region Implementation of IColorSource

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.SolidColor;
			}
		}

		#endregion
	}
}
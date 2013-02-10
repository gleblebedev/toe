namespace Toe.Utils.Mesh
{
	public class FunctionColorSource : IColorSource
	{
		#region Implementation of IColorSource

		public ColorSourceType Type
		{
			get
			{
				return ColorSourceType.Function;
			}
		}

		#endregion
	}
}
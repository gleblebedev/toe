namespace Toe.Utils.Mesh.Svg
{
	public class SvgReaderOptions
	{
		#region Constants and Fields

		private float dpi = 72;

		#endregion

		#region Public Properties

		public float Dpi
		{
			get
			{
				return this.dpi;
			}
			set
			{
				this.dpi = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public float CmToPixels(float f)
		{
			return f * this.dpi / 2.54f;
		}

		public float InchToPixels(float f)
		{
			return f * this.dpi;
		}

		#endregion
	}
}
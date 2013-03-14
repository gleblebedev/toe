namespace Toe.Utils.Mesh.Svg
{
	public class SvgReaderOptions
	{
		private float dpi = 72;

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

		public float InchToPixels(float f)
		{
			return f * dpi;
		}

		public float CmToPixels(float f)
		{
			return f * dpi/2.54f;
		}
	}
}
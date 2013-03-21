using System.Collections.Generic;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgPath
	{
		#region Constants and Fields

		private readonly List<SvgPathSegment> segments = new List<SvgPathSegment>(8);

		#endregion

		#region Public Properties

		public List<SvgPathSegment> Segments
		{
			get
			{
				return this.segments;
			}
		}

		#endregion
	}
}
using System.Collections.Generic;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgPath
	{
		private List<SvgPathSegment> segments = new List<SvgPathSegment>(8);
			
		public List<SvgPathSegment> Segments
		{
			get
			{
				return this.segments;
			}
		}
	}
}
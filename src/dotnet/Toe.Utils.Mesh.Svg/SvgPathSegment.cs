using OpenTK;

namespace Toe.Utils.Mesh.Svg
{
	public struct SvgPathSegment
	{
		#region Constants and Fields

		private SvgPathCommand command;

		private Vector2 cubic0;

		private Vector2 cubic1;

		private Vector2 from;

		private Vector2 to;

		#endregion

		#region Public Properties

		public SvgPathCommand Command
		{
			get
			{
				return this.command;
			}
		}

		public Vector2 Cubic0
		{
			get
			{
				return this.cubic0;
			}
		}

		public Vector2 Cubic1
		{
			get
			{
				return this.cubic1;
			}
		}

		public Vector2 From
		{
			get
			{
				return this.@from;
			}
		}

		public Vector2 To
		{
			get
			{
				return this.to;
			}
		}

		#endregion

		#region Public Methods and Operators

		public static SvgPathSegment Cubic(ref Vector2 from, ref Vector2 p0, ref Vector2 p1, ref Vector2 to)
		{
			return new SvgPathSegment { command = SvgPathCommand.CubicBezier, @from = from, cubic0 = p0, cubic1 = p1, to = to };
		}

		public static SvgPathSegment Line(ref Vector2 from, ref Vector2 to)
		{
			var p = (1.0f / 2.0f) * from + (1.0f / 2.0f) * to;
			var p0 = (1.0f / 3.0f) * from + (2.0f / 3.0f) * p;
			var p1 = (2.0f / 3.0f) * p + (1.0f / 3.0f) * to;
			return new SvgPathSegment { command = SvgPathCommand.LineTo, @from = from, cubic0 = p0, cubic1 = p1, to = to };
		}

		public static SvgPathSegment Quadratic(ref Vector2 from, ref Vector2 p, ref Vector2 to)
		{
			var p0 = (1.0f / 3.0f) * from + (2.0f / 3.0f) * p;
			var p1 = (2.0f / 3.0f) * p + (1.0f / 3.0f) * to;
			return new SvgPathSegment
				{ command = SvgPathCommand.QuadraticBezier, cubic0 = p0, cubic1 = p1, @from = from, to = to };
		}

		#endregion
	}
}
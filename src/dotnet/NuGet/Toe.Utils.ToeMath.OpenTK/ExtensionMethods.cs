using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Toe.Utils.ToeMath
{
    public static class ExtensionMethods
    {
		public static Vector3 ToVector(this Float3 f) { return new Vector3(f.X,f.Y,f.Z);}
		public static Float3 ToFloat3(this Vector3 f) { return new Float3(f.X, f.Y, f.Z); }
		public static Matrix4 ToMatrix(this Float4x4 f) { return new Matrix4(f.M00, f.M01, f.M02, f.M03,
			f.M10, f.M11, f.M12, f.M13,
			f.M20, f.M21, f.M22, f.M23,
			f.M30, f.M31, f.M32, f.M33);
		}
    }
}

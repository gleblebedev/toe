using OpenTK;

namespace Toe.Gx
{
	public struct NormalizedPlane
	{
		#region Constants and Fields

		private float A, B, C, D;

		#endregion

		#region Public Methods and Operators

		public static void BuildPlane(ref Vector3 n, ref Vector3 p, out NormalizedPlane plane)
		{
			plane.A = n.X;
			plane.B = n.Y;
			plane.C = n.Z;
			Vector3.Dot(ref n, ref p, out plane.D);
			plane.D = -plane.D;
		}

		public static void BuildPlane(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2, out NormalizedPlane plane)
		{
			Vector3 n = Vector3.Cross((p1 - p0), (p2 - p0));
			n.Normalize();
			BuildPlane(ref n, ref p0, out plane);
		}

		public bool CheckSphere(ref Vector3 boundingSphereCenter, ref float boundingSphereR)
		{
			var d = boundingSphereCenter.X * this.A + boundingSphereCenter.Y * this.B + boundingSphereCenter.Z * this.C + this.D;
			return (d <= boundingSphereR);
		}

		#endregion
	}
}
using OpenTK;

namespace Toe.Gx
{
	public struct Frustum
	{
		private NormalizedPlane Front;

		private NormalizedPlane Back;

		private NormalizedPlane Right;

		private NormalizedPlane Right2;

		private NormalizedPlane Right3;

		private NormalizedPlane Right4;

		private Vector3 cameraPosition;

		public Vector3 CameraPosition
		{
			get
			{
				return this.cameraPosition;
			}
		}

		public static void BuildFrustum(ref Matrix4 view, ref Matrix4 projection, out Frustum frustum)
		{
			frustum = new Frustum();

			var m = view;
			m.Invert();
			frustum.cameraPosition = new Vector3(m.M41, m.M42, m.M43);

			m = view * projection;
			m.Invert();
			Vector3 farA, farB, farC, farD;
			Vector3 nearA, nearB, nearC, nearD;

			PointFromProjectionToWorld(new Vector4(1, 1, 1, 1), ref m, out farA);
			PointFromProjectionToWorld(new Vector4(-1, 1, 1, 1), ref m, out farB);
			PointFromProjectionToWorld(new Vector4(-1, -1, 1, 1), ref m, out farC);
			PointFromProjectionToWorld(new Vector4(1, -1, 1, 1), ref m, out farD);

			PointFromProjectionToWorld(new Vector4(1, 1, -1, 1), ref m, out nearA);
			PointFromProjectionToWorld(new Vector4(-1, 1, -1, 1), ref m, out nearB);
			PointFromProjectionToWorld(new Vector4(-1, -1, -1, 1), ref m, out nearC);
			PointFromProjectionToWorld(new Vector4(1, -1, -1, 1), ref m, out nearD);

			NormalizedPlane.BuildPlane(ref nearA, ref nearB, ref nearC, out frustum.Front);
			NormalizedPlane.BuildPlane(ref farA, ref farC, ref farB, out frustum.Back);

			NormalizedPlane.BuildPlane(ref nearA, ref farB, ref nearB, out frustum.Right);
			NormalizedPlane.BuildPlane(ref nearB, ref farC, ref nearC, out frustum.Right2);
			NormalizedPlane.BuildPlane(ref nearC, ref farD, ref nearD, out frustum.Right3);
			NormalizedPlane.BuildPlane(ref nearD, ref farA, ref nearA, out frustum.Right4);
		}

		private static void PointFromProjectionToWorld(Vector4 vector3, ref Matrix4 m, out Vector3 a)
		{
			Vector4 tmp;
			Vector4.Transform(ref vector3, ref m, out tmp);
			a = new Vector3(tmp.X / tmp.W, tmp.Y / tmp.W, tmp.Z / tmp.W);
		}

		public bool CheckSphere(Vector3 boundingSphereCenter, float boundingSphereR)
		{
			if (!Front.CheckSphere(ref boundingSphereCenter, ref boundingSphereR)) return false;
			if (!Back.CheckSphere(ref boundingSphereCenter, ref boundingSphereR)) return false;
			if (!Right.CheckSphere(ref boundingSphereCenter, ref boundingSphereR)) return false;
			if (!Right2.CheckSphere(ref boundingSphereCenter, ref boundingSphereR)) return false;
			if (!Right3.CheckSphere(ref boundingSphereCenter, ref boundingSphereR)) return false;
			if (!Right4.CheckSphere(ref boundingSphereCenter, ref boundingSphereR)) return false;
			return true;
		}
	}
}
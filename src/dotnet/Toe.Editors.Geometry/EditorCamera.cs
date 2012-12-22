using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Toe.Editors.Geometry
{
	public class EditorCamera
	{
		#region Constants and Fields

		private float aspectRation = 1;

		private float fovy = (float)Math.PI / 2.0f;

		private bool ortho = true;

		private Vector3 pos = new Vector3(0, 0, 1024);

		private Quaternion rot = Quaternion.Identity;

		private float zFar = 2048.0f;

		private float zNear = 0.1f;

		#endregion

		#region Public Properties

		public float AspectRation
		{
			get
			{
				return this.aspectRation;
			}
			set
			{
				this.aspectRation = value;
			}
		}

		public bool Ortho
		{
			get
			{
				return this.ortho;
			}
			set
			{
				this.ortho = value;
			}
		}

		public Vector3 Pos
		{
			get
			{
				return this.pos;
			}
			set
			{
				this.pos = value;
			}
		}

		public Quaternion Rot
		{
			get
			{
				return this.rot;
			}
			set
			{
				this.rot = value;
			}
		}

		public float ZFar
		{
			get
			{
				return this.zFar;
			}
			set
			{
				this.zFar = value;
			}
		}

		public float ZNear
		{
			get
			{
				return this.zNear;
			}
			set
			{
				this.zNear = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Creates an orthographic projection matrix.
		/// </summary>
		/// <param name="left">The left edge of the projection volume.</param>
		/// <param name="right">The right edge of the projection volume.</param>
		/// <param name="bottom">The bottom edge of the projection volume.</param>
		/// <param name="top">The top edge of the projection volume.</param>
		/// <param name="zNear">The near edge of the projection volume.</param>
		/// <param name="zFar">The far edge of the projection volume.</param>
		/// <param name="result">The resulting Matrix4 instance.</param>
		public static void CreateOrthographicOffCenter(
			float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
		{
			result = new Matrix4();

			float invRL = 1 / (right - left);
			float invTB = 1 / (top - bottom);
			float invFN = 1 / (zFar - zNear);

			result.M11 = 2 * invRL;
			result.M22 = 2 * invTB;
			result.M33 = -2 * invFN;

			result.M41 = -(right + left) * invRL;
			result.M42 = -(top + bottom) * invTB;
			result.M43 = -(zFar + zNear) * invFN;
			result.M44 = 1;
		}

		/// <summary>
		/// Creates a perspective projection matrix.
		/// </summary>
		/// <param name="fovy">Angle of the field of view in the y direction (in radians)</param>
		/// <param name="aspect">Aspect ratio of the view (width / height)</param>
		/// <param name="zNear">Distance to the near clip plane</param>
		/// <param name="zFar">Distance to the far clip plane</param>
		/// <param name="result">A projection matrix that transforms camera space to raster space</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown under the following conditions:
		/// <list type="bullet">
		/// <item>fovy is zero, less than zero or larger than Math.PI</item>
		/// <item>aspect is negative or zero</item>
		/// <item>zNear is negative or zero</item>
		/// <item>zFar is negative or zero</item>
		/// <item>zNear is larger than zFar</item>
		/// </list>
		/// </exception>
		public static void CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar, out Matrix4 result)
		{
			if (fovy <= 0 || fovy > Math.PI)
			{
				throw new ArgumentOutOfRangeException("fovy");
			}
			if (aspect <= 0)
			{
				throw new ArgumentOutOfRangeException("aspect");
			}
			if (zNear <= 0)
			{
				throw new ArgumentOutOfRangeException("zNear");
			}
			if (zFar <= 0)
			{
				throw new ArgumentOutOfRangeException("zFar");
			}

			float yMax = zNear * (float)Math.Tan(0.5f * fovy);
			float yMin = -yMax;
			float xMin = yMin * aspect;
			float xMax = yMax * aspect;

			CreatePerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar, out result);
		}

		/// <summary>
		/// Creates an perspective projection matrix.
		/// </summary>
		/// <param name="left">Left edge of the view frustum</param>
		/// <param name="right">Right edge of the view frustum</param>
		/// <param name="bottom">Bottom edge of the view frustum</param>
		/// <param name="top">Top edge of the view frustum</param>
		/// <param name="zNear">Distance to the near clip plane</param>
		/// <param name="zFar">Distance to the far clip plane</param>
		/// <param name="result">A projection matrix that transforms camera space to raster space</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown under the following conditions:
		/// <list type="bullet">
		/// <item>zNear is negative or zero</item>
		/// <item>zFar is negative or zero</item>
		/// <item>zNear is larger than zFar</item>
		/// </list>
		/// </exception>
		public static void CreatePerspectiveOffCenter(
			float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4 result)
		{
			if (zNear <= 0)
			{
				throw new ArgumentOutOfRangeException("zNear");
			}
			if (zFar <= 0)
			{
				throw new ArgumentOutOfRangeException("zFar");
			}
			if (zNear >= zFar)
			{
				throw new ArgumentOutOfRangeException("zNear");
			}

			float x = (2.0f * zNear) / (right - left);
			float y = (2.0f * zNear) / (top - bottom);
			float a = (right + left) / (right - left);
			float b = (top + bottom) / (top - bottom);
			float c = -(zFar + zNear) / (zFar - zNear);
			float d = -(2.0f * zFar * zNear) / (zFar - zNear);

			result = new Matrix4(x, 0, 0, 0, 0, y, 0, 0, a, b, c, -1, 0, 0, d, 0);
		}

		/// <summary>
		/// Build a world space to camera space matrix
		/// </summary>
		/// <param name="eye">Eye (camera) position in world space</param>
		/// <param name="target">Target position in world space</param>
		/// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
		/// <returns>A Matrix4 that transforms world space to camera space</returns>
		public void LookAt(Vector3 eye, Vector3 target, Vector3 up)
		{
			this.pos = eye;
			//Vector3 z = Vector3.Normalize(eye - target);
			//Vector3 x = Vector3.Normalize(Vector3.Cross(up, z));
			//Vector3 y = Vector3.Normalize(Vector3.Cross(z, x));

			//Matrix4 mrot = new Matrix4(new Vector4(x.X, y.X, z.X, 0.0f), new Vector4(x.Y, y.Y, z.Y, 0.0f), new Vector4(x.Z, y.Z, z.Z, 0.0f), Vector4.UnitW);

			Vector3 z = Vector3.Normalize(target - eye);
			Vector3 x = Vector3.Normalize(Vector3.Cross(up, z));
			Vector3 y = Vector3.Normalize(Vector3.Cross(z, x));

			this.rot = QuaternionFromBasis(-x, y, -z);
			//this.rot.Conjugate();
		}

		public void SetProjection()
		{
			GL.MatrixMode(MatrixMode.Projection);
			Matrix4 projection;
			if (this.ortho)
			{
				float w = 1024;
				float h = 1024 / this.aspectRation;
				CreateOrthographicOffCenter(-w / 2, w / 2, -h / 2, h / 2, -this.zFar, this.zFar, out projection);
			}
			else
			{
				CreatePerspectiveFieldOfView(this.fovy, this.aspectRation, this.zNear, this.zFar, out projection);
			}
			GL.LoadMatrix(ref projection);

			GL.MatrixMode(MatrixMode.Modelview);

			Matrix4 view = Matrix4.Rotate(this.rot) * Matrix4.CreateTranslation(this.pos);
			view.Invert();
			GL.LoadMatrix(ref view);
		}

		#endregion

		#region Methods

		private static Quaternion QuaternionFromBasis(Vector3 x, Vector3 y, Vector3 z)
		{
			Quaternion rot = Quaternion.Identity;
			rot.W = (float)Math.Sqrt(1.0f + x.X + y.Y + z.Z) * 0.5f;
			float w4_recip = 1.0f / (4.0f * rot.W);
			rot.X = (y.Z - z.Y) * w4_recip;
			rot.Y = (z.X - x.Z) * w4_recip;
			rot.Z = (x.Y - y.X) * w4_recip;
			rot.Normalize();
			return rot;
		}

		#endregion
	}
}
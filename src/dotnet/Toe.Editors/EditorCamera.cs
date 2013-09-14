using System;

using OpenTK;

using Toe.Editors.Interfaces;
using Toe.Gx;
using Toe.Utils;

namespace Toe.Editors
{
	public class EditorCamera : ClassWithNotification
	{
		#region Constants and Fields

		protected static PropertyEventArgs AspectRatioEventArgs = Expr.PropertyEventArgs<EditorCamera>(x => x.AspectRatio);

		protected static PropertyEventArgs CoordinateSystemEventArgs =
			Expr.PropertyEventArgs<EditorCamera>(x => x.CoordinateSystem);

		protected static PropertyEventArgs PosEventArgs = Expr.PropertyEventArgs<EditorCamera>(x => x.Pos);

		protected static PropertyEventArgs RotEventArgs = Expr.PropertyEventArgs<EditorCamera>(x => x.Rot);

		private readonly IEditorOptions<EditorCameraOptions> cameraOptions;

		private float aspectRatio = 1;

		private CoordinateSystem coordinateSystem = CoordinateSystem.ZUp;

		private float fovy = (float)Math.PI / 2.0f;

		private bool ortho;

		private Vector3 pos = new Vector3(0, 0, 1024);

		private Quaternion rot = Quaternion.Identity;

		private float zFar = 4 * 2048.0f;

		private float zNear = 16.0f;

		#endregion

		#region Constructors and Destructors

		public EditorCamera(IEditorOptions<EditorCameraOptions> cameraOptions)
		{
			this.cameraOptions = cameraOptions;
			this.coordinateSystem = cameraOptions.Options.CoordinateSystem;
		}

		#endregion

		#region Public Properties

		public float AspectRatio
		{
			get
			{
				return this.aspectRatio;
			}
			set
			{
				if (this.aspectRatio != value)
				{
					this.RaisePropertyChanging(AspectRatioEventArgs.Changing);
					this.aspectRatio = value;
					this.RaisePropertyChanged(AspectRatioEventArgs.Changed);
				}
			}
		}

		public CoordinateSystem CoordinateSystem
		{
			get
			{
				return this.coordinateSystem;
			}
			set
			{
				if (this.coordinateSystem != value)
				{
					this.RaisePropertyChanging(CoordinateSystemEventArgs.Changing);
					this.coordinateSystem = value;
					this.cameraOptions.Options.CoordinateSystem = value;
					this.cameraOptions.Save();
					this.LookAt(this.pos, this.pos + this.Forward);
					this.RaisePropertyChanged(CoordinateSystemEventArgs.Changed);
				}
			}
		}

		public Vector3 Forward
		{
			get
			{
				return Vector3.Transform(new Vector3(0, 0, -1), this.rot);
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
				if (this.pos != value)
				{
					this.RaisePropertyChanging(PosEventArgs.Changing);
					this.pos = value;
					this.RaisePropertyChanged(PosEventArgs.Changed);
				}
			}
		}

		public Vector3 Right
		{
			get
			{
				return Vector3.Transform(new Vector3(1, 0, 0), this.rot);
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
				if (this.rot != value)
				{
					this.RaisePropertyChanging(RotEventArgs.Changing);
					this.rot = value;
					this.RaisePropertyChanged(RotEventArgs.Changed);
				}
			}
		}

		public Vector3 Up
		{
			get
			{
				return Vector3.Transform(new Vector3(0, 1, 0), this.rot);
			}
		}

		public Vector3 WorldForward
		{
			get
			{
				switch (this.coordinateSystem)
				{
					case CoordinateSystem.ZUp:
						return new Vector3(0, 1, 0);
					case CoordinateSystem.YUp:
						return new Vector3(0, 0, -1);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public Vector3 WorldRight
		{
			get
			{
				switch (this.coordinateSystem)
				{
					case CoordinateSystem.ZUp:
						return new Vector3(1, 0, 0);
					case CoordinateSystem.YUp:
						return new Vector3(1, 0, 0);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public Vector3 WorldUp
		{
			get
			{
				switch (this.coordinateSystem)
				{
					case CoordinateSystem.ZUp:
						return new Vector3(0, 0, 1);
					case CoordinateSystem.YUp:
						return new Vector3(0, 1, 0);
					default:
						throw new ArgumentOutOfRangeException();
				}
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
			this.RaisePropertyChanging(RotEventArgs.Changing);
			this.RaisePropertyChanging(PosEventArgs.Changing);

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

			this.RaisePropertyChanged(RotEventArgs.Changed);
			this.RaisePropertyChanged(PosEventArgs.Changed);
		}

		/// <summary>
		/// Build a world space to camera space matrix
		/// </summary>
		/// <param name="eye">Eye (camera) position in world space</param>
		/// <param name="target">Target position in world space</param>
		/// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
		/// <returns>A Matrix4 that transforms world space to camera space</returns>
		public void LookAt(Vector3 eye, Vector3 target)
		{
			LookAt(eye,target,WorldUp);
		}

		public void SetProjection(ToeGraphicsContext graphicsContext)
		{
			Matrix4 projection;
			if (this.ortho)
			{
				float w = 1024;
				float h = 1024 / this.aspectRatio;
				CreateOrthographicOffCenter(-w / 2, w / 2, -h / 2, h / 2, -this.zFar, this.zFar, out projection);
			}
			else
			{
				CreatePerspectiveFieldOfView(this.fovy, this.aspectRatio, this.zNear, this.zFar, out projection);
			}

			graphicsContext.SetProjection(ref projection);

			Matrix4 view = Matrix4.Rotate(this.rot) * Matrix4.CreateTranslation(this.pos);
			view.Invert();

			graphicsContext.SetView(ref view);
		}

		#endregion

		#region Methods

		private static Quaternion QuaternionFromBasis(Vector3 x, Vector3 y, Vector3 z)
		{
			Quaternion rot = Quaternion.Identity;
			var T = 1.0f + x.X + y.Y + z.Z;
			if (T > 0)
			{
				rot.W = (float)Math.Sqrt(T) * 0.5f;
				float w4_recip = 1.0f / (4.0f * rot.W);
				rot.X = (y.Z - z.Y) * w4_recip;
				rot.Y = (z.X - x.Z) * w4_recip;
				rot.Z = (x.Y - y.X) * w4_recip;
				rot.Normalize();
				return rot;
			}
			else
			{
				var index = 0;
				var value = (x.X);
				if ((y.Y) > value)
				{
				index =1;
				value = (y.Y);
				}
				if ((z.Z) > value)
				{
				index =2;
				}
				switch (index)
				{
					case 0:
						{
							var S = (float)Math.Sqrt(1.0 + x.X - y.Y - z.Z) * 2;

							rot.X = 0.5f / S;
							rot.Y = (y.X + x.Y) / S;
							rot.Z = (z.X + x.Z) / S;
							rot.W = (z.Y + y.Z) / S;
						}
						break;
					case 1:
						{
							var S = (float)Math.Sqrt( 1.0 + y.Y - x.X - z.Z ) * 2;

				rot.X = (y.X + x.Y ) / S;
				rot.Y = 0.5f / S;
				rot.Z = (z.Y + y.Z ) / S;
				rot.W = (z.X + x.Z ) / S;
						}
						break;
					case 2:
						{
							var S= (float)Math.Sqrt( 1.0 + z.Z - x.X - y.Y ) * 2;

				rot.X = (z.X + x.Z ) / S;
				rot.Y = (z.Y + y.Z ) / S;
				rot.Z = 0.5f / S;
				rot.W = (y.X + x.Y ) / S;
						}
						break;

				}

				rot.Normalize();
				return rot;
			}
		}

		#endregion

		
	}
}
using System.Drawing;

using OpenTK;

namespace Toe.Gx
{
	public struct LightArgs
	{
		#region Constants and Fields

		public static readonly LightArgs Default = new LightArgs
			{
				QuadraticAttenuation = 0.0f,
				LinearAttenuation = 0.0f,
				ConstantAttenuation = 1.0f,
				Cutoff = 180.0f,
				Direction = new Vector3(0, 0, -1),
				Position = new Vector3(0, 0, 1),
				Specular = Color.FromArgb(255, 255, 255, 255),
				Diffuse = Color.FromArgb(255, 255, 255, 255),
				Ambient = Color.FromArgb(255, 0, 0, 0),
				Enabled = false
			};

		public Color Ambient;

		public float ConstantAttenuation;

		public float Cutoff;

		public Color Diffuse;

		public Vector3 Direction;

		public bool Enabled;

		public float Exponent;

		public float LinearAttenuation;

		public Vector3 Position;

		public float QuadraticAttenuation;

		public Color Specular;

		#endregion
	}
}
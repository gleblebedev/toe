using System.Drawing;

using OpenTK;

namespace Toe.Gx
{
	public struct LightArgs
	{
		public bool Enabled;
		public Color Ambient;
		public Color Diffuse;
		public Color Specular;
		public Vector3 Position;
		public Vector3 Direction;
		public float Exponent;
		public float Cutoff;
		public float ConstantAttenuation;
		public float LinearAttenuation;
		public float QuadraticAttenuation;

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
	}
}
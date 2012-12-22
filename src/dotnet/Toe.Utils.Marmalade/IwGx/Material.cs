using System.Drawing;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class Material : Managed
	{
		#region Public Properties

		public float CelH { get; set; }

		public float CelW { get; set; }

		public string alphaMode { get; set; }

		public string blendMode { get; set; }

		public float celNum { get; set; }

		public float celNumU { get; set; }

		public float celPeriod { get; set; }

		public bool clampUV { get; set; }

		public Color colAmbient { get; set; }

		public Color colDiffuse { get; set; }

		public Color colEmissive { get; set; }

		public Color colSpecular { get; set; }

		public string cullMode { get; set; }

		public string effectPreset { get; set; }

		public string shaderTechnique { get; set; }

		public float specularPower { get; set; }

		public string texture0 { get; set; }

		public string texture1 { get; set; }

		public string texture2 { get; set; }

		public string vertexShader { get; set; }

		#endregion
	}
}
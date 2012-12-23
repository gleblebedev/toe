using System.ComponentModel;
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

		public int specularPower { get; set; }

		private string texture0;

		public string Texture0
		{
			get
			{
				return this.texture0;
			}
			set
			{
				if (this.texture0 != value)
				{
					this.texture0 = value;
					this.RaisePropertyChanged("Texture1");
				}
			}
		}

		private string texture1;

		public string Texture1
		{
			get
			{
				return this.texture1;
			}
			set
			{
				if (this.texture1 != value)
				{
					this.texture1 = value;
					this.RaisePropertyChanged("Texture1");
				}
			}
		}

		public string texture2 { get; set; }

		public string vertexShader { get; set; }

		public ShadeMode shadeMode { get; set; }

		#endregion

		
	}
}
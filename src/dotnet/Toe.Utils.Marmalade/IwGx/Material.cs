using System.ComponentModel;
using System.Drawing;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class Material : Managed
	{
		#region Public Properties


		private MatAnim matAnim;

		public MatAnim MatAnim
		{
			get
			{
				return this.matAnim;
			}
			set
			{
				this.matAnim = value;
			}
		}

		private AlphaTestMode alphaTestMode = AlphaTestMode.ALPHATEST_DISABLED;

		public AlphaTestMode AlphaTestMode
		{
			get
			{
				return this.alphaTestMode;
			}
			set
			{
				this.alphaTestMode = value;
			}
		}


		private AlphaMode alphaMode = AlphaMode.DEFAULT;

		public AlphaMode AlphaMode
		{
			get
			{
				return this.alphaMode;
			}
			set
			{
				this.alphaMode = value;
			}
		}

		private BlendMode blendMode = BlendMode.BLEND_BLEND;

		public BlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
			}
		}

		

		public bool ClampUV { get; set; }

		private Color colAmbient = Color.FromArgb(255,255,255,255);

		public Color ColAmbient
		{
			get
			{
				return this.colAmbient;
			}
			set
			{
				this.colAmbient = value;
			}
		}

		private Color colDiffuse = Color.FromArgb(255, 255, 255, 255);

		public Color ColDiffuse
		{
			get
			{
				return this.colDiffuse;
			}
			set
			{
				this.colDiffuse = value;
			}
		}

		private Color colEmissive = Color.FromArgb(0, 0, 0, 0);

		public Color ColEmissive
		{
			get
			{
				return this.colEmissive;
			}
			set
			{
				this.colEmissive = value;
			}
		}

		private Color colSpecular = Color.FromArgb(10, 0, 0, 0);

		public Color ColSpecular
		{
			get
			{
				return this.colSpecular;
			}
			set
			{
				this.colSpecular = value;
			}
		}

		private CullMode cullMode;

		public CullMode CullMode
		{
			get
			{
				return this.cullMode;
			}
			set
			{
				this.cullMode = value;
			}
		}

		private EffectPreset effectPreset = EffectPreset.DEFAULT;

		public EffectPreset EffectPreset
		{
			get
			{
				return this.effectPreset;
			}
			set
			{
				this.effectPreset = value;
			}
		}

		public string ShaderTechnique { get; set; }

		private int specularPower;

		public int SpecularPower
		{
			get
			{
				return specularPower;
			}
			set
			{
				specularPower = value;
			}
		}

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

		private string texture2;

		public string Texture2
		{
			get
			{
				return this.texture2;
			}
			set
			{
				if (this.texture2 != value)
				{
					this.texture2 = value;
					this.RaisePropertyChanged("Texture2");
				}
			}
		}

		private string texture3;

		public string Texture3
		{
			get
			{
				return this.texture3;
			}
			set
			{
				if (this.texture3 != value)
				{
					this.texture3 = value;
					this.RaisePropertyChanged("Texture3");
				}
			}
		}

		public string vertexShader { get; set; }

		private ShadeMode shadeMode = ShadeMode.SHADE_GOURAUD;

		private ModulateMode modulateMode = ModulateMode.RGB;

		private byte alphaTestValue;

		private short zDepthOfsHw;

		private bool invisible;

		private bool filtering;

		public ShadeMode ShadeMode
		{
			get
			{
				return this.shadeMode;
			}
			set
			{
				this.shadeMode = value;
			}
		}

		public ModulateMode ModulateMode
		{
			get
			{
				return modulateMode;
			}
			set
			{
				modulateMode = value;
			}
		}

		public byte AlphaTestValue
		{
			get
			{
				return alphaTestValue;
			}
			set
			{
				alphaTestValue = value;
			}
		}

		private short zDepthOfs;

		private bool depthWriteEnable = true;

		private bool mergeGeom;

		private ImageFormat formatHw;

		private ImageFormat formatSw;

		private bool keepAfterUpload;

		public short ZDepthOfs
		{
			get
			{
				return zDepthOfs;
			}
			set
			{
				zDepthOfs = value;
			}
		}

		public short ZDepthOfsHW
		{
			get
			{
				return zDepthOfsHw;
			}
			set
			{
				zDepthOfsHw = value;
			}
		}

		public bool Invisible
		{
			get
			{
				return this.invisible;
			}
			set
			{
				this.invisible = value;
			}
		}

		public bool Filtering
		{
			get
			{
				return filtering;
			}
			set
			{
				filtering = value;
			}
		}

		public bool DepthWriteEnable
		{
			get
			{
				return depthWriteEnable;
			}
			set
			{
				depthWriteEnable = value;
			}
		}

		public bool MergeGeom
		{
			get
			{
				return mergeGeom;
			}
			set
			{
				mergeGeom = value;
			}
		}

		public ImageFormat FormatHW
		{
			get
			{
				return formatHw;
			}
			set
			{
				formatHw = value;
			}
		}

		public ImageFormat FormatSW
		{
			get
			{
				return formatSw;
			}
			set
			{
				formatSw = value;
			}
		}

		public bool KeepAfterUpload
		{
			get
			{
				return keepAfterUpload;
			}
			set
			{
				keepAfterUpload = value;
			}
		}

		private bool noFog;

		public bool NoFog
		{
			get
			{
				return this.noFog;
			}
			set
			{
				this.noFog = value;
			}
		}

		#endregion

		
	}
}
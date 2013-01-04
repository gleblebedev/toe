using System;
using System.ComponentModel;
using System.Drawing;

using OpenTK.Graphics.OpenGL;

using Toe.Gx;
using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.IwGx
{
	public class Material : Managed
	{
		/// <summary>
		/// if gouraud-shaded, perform intensity shading only, using R component of colour
		/// </summary>
		public const uint INTENSITY_F = (1 << 0);

		/// <summary>
		/// No shading or lighting, fully lit
		/// </summary>
		public const uint UNMODULATE_F = (1 << 1);
        public const uint FLAT_F          = (1 << 2);     // do NOT use gouraud shading for this material
        public const uint TWO_SIDED_F     = (1 << 3);     // material is two-sided (perform no culling)
        public const uint CULL_FRONT_F    = (1 << 4);     // if TWO_SIDED_F is NOT set; cull front-facing rather than back-facing polys
        public const uint NO_FILTERING_F  = (1 << 5);     // disable filtering for all textures
        public const uint NO_RENDER_F     = (1 << 6);     // do not render geometry with this material
        public const uint MERGE_GEOM_F    = (1 << 7);     // If trans=SW; light=SW; rast=HW: try to merge all MatGeomInfo into one
        public const uint CLAMP_UV_F      = (1 << 8);     // If set the texture coords are clamped when the texture object is set
        public const uint ATLAS_MATERIAL_F =  (1 << 9);
        public const uint NO_FOG_F        = (1 <<10);     // Disable Fogging for this material

        // System flags
        public const uint IN_USE_F        = (1 << 12);    // material is being used to render this frame (used when caching geometry)



        // Packed Enum Members
        public const int ALPHA_MODE_SHIFT    = 16;
        public const uint ALPHA_MODE_MASK     = 0x00070000;   // AlphaMode member
        public const int BLEND_MODE_SHIFT    = 19;
        public const uint BLEND_MODE_MASK     = 0x00380000;   // BlendMode member
        public const int EFFECT_PRESET_SHIFT = 22;
        public const uint EFFECT_PRESET_MASK  = 0x01C00000;   // EffectPreset member
        public const int ALPHATEST_MODE_SHIFT = 25;
        public const uint ALPHATEST_MODE_MASK = 0x1e000000;   // Alpha Test member
        public const int DEPTH_WRITE_MODE_SHIFT = 29;

		/// <summary>
		/// Depth test mode member
		/// </summary>
		public const uint DEPTH_WRITE_MODE_MASK = 0x1 << DEPTH_WRITE_MODE_SHIFT; 

		public const uint PRIVATE_FLAGS_MASK = 0xffffffff;


		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwMaterial");

		private readonly ToeGraphicsContext graphicsContext;

		private readonly IResourceManager resourceManager;

		private readonly ResourceReference shaderTechnique;

		private readonly ResourceReference texture0;

		private readonly ResourceReference texture1;

		private readonly ResourceReference texture2;

		private readonly ResourceReference texture3;

		private AlphaMode alphaMode = AlphaMode.DEFAULT;

		private AlphaTestMode alphaTestMode = AlphaTestMode.DISABLED;

		private byte alphaTestValue;

		private BlendMode blendMode = BlendMode.MODULATE;

		private bool clampUV;

		private Color colAmbient = Color.FromArgb(255, 255, 255, 255);

		private Color colDiffuse = Color.FromArgb(255, 255, 255, 255);

		private Color colEmissive = Color.FromArgb(0, 0, 0, 0);

		private Color colSpecular = Color.FromArgb(10, 0, 0, 0);

		private CullMode cullMode = CullMode.BACK;

		private bool depthWriteEnable = true;

		private EffectPreset effectPreset = EffectPreset.DEFAULT;

		private bool filtering = true;

		private ImageFormat formatHw;

		private ImageFormat formatSw;

		private bool invisible;

		private bool keepAfterUpload;

		private MatAnim matAnim;

		private bool mergeGeom;

		private ModulateMode modulateMode = ModulateMode.RGB;

		private bool noFog = false;

		private ShadeMode shadeMode = ShadeMode.GOURAUD;

		private int specularPower = 10;

		private string vertexShader;

		private int zDepthOfs;

		private int zDepthOfsHw;

		private bool atlasMaterial;

		#endregion

		#region Constructors and Destructors

		public Material(IResourceManager resourceManager, ToeGraphicsContext graphicsContext)
		{
			this.resourceManager = resourceManager;
			this.graphicsContext = graphicsContext;
			this.texture0 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture0.ReferenceChanged += (s, a) => this.RaisePropertyChanged("Texture0");

			this.texture1 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture1.ReferenceChanged += (s, a) => this.RaisePropertyChanged("Texture1");

			this.texture2 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture2.ReferenceChanged += (s, a) => this.RaisePropertyChanged("Texture2");

			this.texture3 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture3.ReferenceChanged += (s, a) => this.RaisePropertyChanged("Texture3");

			this.shaderTechnique = new ResourceReference(Utils.Marmalade.IwGx.ShaderTechnique.TypeHash, resourceManager, this);
			this.shaderTechnique.ReferenceChanged += (s, a) => this.RaisePropertyChanged("ShaderTechnique");
		}

		#endregion

		#region Public Properties

		public AlphaMode AlphaMode
		{
			get
			{
				return this.alphaMode;
			}
			set
			{
				if (this.alphaMode != value)
				{
					this.alphaMode = value;
					this.RaisePropertyChanged("AlphaMode");
				}
			}
		}

		public AlphaTestMode AlphaTestMode
		{
			get
			{
				return this.alphaTestMode;
			}
			set
			{
				if (this.alphaTestMode != value)
				{
					this.alphaTestMode = value;
					this.RaisePropertyChanged("AlphaTestMode");
				}
			}
		}

		public byte AlphaTestValue
		{
			get
			{
				return this.alphaTestValue;
			}
			set
			{
				if (this.alphaTestValue != value)
				{
					this.alphaTestValue = value;
					this.RaisePropertyChanged("AlphaTestValue");
				}
			}
		}

		public BlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				if (this.blendMode != value)
				{
					this.blendMode = value;
					this.RaisePropertyChanged("BlendMode");
				}
			}
		}

		public bool ClampUV
		{
			get
			{
				return this.clampUV;
			}
			set
			{
				if (this.clampUV != value)
				{
					this.clampUV = value;
					this.RaisePropertyChanged("ClampUV");
				}
			}
		}

		public Color ColAmbient
		{
			get
			{
				return this.colAmbient;
			}
			set
			{
				if (this.colAmbient != value)
				{
					this.colAmbient = value;
					this.RaisePropertyChanged("ColAmbient");
				}
			}
		}

		public Color ColDiffuse
		{
			get
			{
				return this.colDiffuse;
			}
			set
			{
				if (this.colDiffuse != value)
				{
					this.colDiffuse = value;
					this.RaisePropertyChanged("ColDiffuse");
				}
			}
		}

		public Color ColEmissive
		{
			get
			{
				return this.colEmissive;
			}
			set
			{
				if (this.colEmissive != value)
				{
					this.colEmissive = value;
					this.RaisePropertyChanged("ColEmissive");
				}
			}
		}

		public Color ColSpecular
		{
			get
			{
				return this.colSpecular;
			}
			set
			{
				if (this.colSpecular != value)
				{
					this.colSpecular = value;
					this.RaisePropertyChanged("ColSpecular");
				}
			}
		}

		public CullMode CullMode
		{
			get
			{
				return this.cullMode;
			}
			set
			{
				if (this.cullMode != value)
				{
					this.cullMode = value;
					this.RaisePropertyChanged("CullMode");
				}
			}
		}

		public bool DepthWriteEnable
		{
			get
			{
				return this.depthWriteEnable;
			}
			set
			{
				if (this.depthWriteEnable != value)
				{
					this.depthWriteEnable = value;
					this.RaisePropertyChanged("DepthWriteEnable");
				}
			}
		}

		public EffectPreset EffectPreset
		{
			get
			{
				return this.effectPreset;
			}
			set
			{
				if (this.effectPreset != value)
				{
					this.effectPreset = value;
					this.RaisePropertyChanged("EffectPreset");
				}
			}
		}

		public bool Filtering
		{
			get
			{
				return this.filtering;
			}
			set
			{
				if (this.filtering != value)
				{
					this.filtering = value;
					this.RaisePropertyChanged("Filtering");
				}
			}
		}

		public ImageFormat FormatHW
		{
			get
			{
				return this.formatHw;
			}
			set
			{
				if (this.formatHw != value)
				{
					this.formatHw = value;
					this.RaisePropertyChanged("FormatHW");
				}
			}
		}

		public ImageFormat FormatSW
		{
			get
			{
				return this.formatSw;
			}
			set
			{
				if (this.formatSw != value)
				{
					this.formatSw = value;
					this.RaisePropertyChanged("FormatSW");
				}
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
				if (this.invisible != value)
				{
					this.invisible = value;
					this.RaisePropertyChanged("Invisible");
				}
			}
		}

		public bool KeepAfterUpload
		{
			get
			{
				return this.keepAfterUpload;
			}
			set
			{
				if (this.keepAfterUpload != value)
				{
					this.keepAfterUpload = value;
					this.RaisePropertyChanged("KeepAfterUpload");
				}
			}
		}

		public MatAnim MatAnim
		{
			get
			{
				return this.matAnim;
			}
			set
			{
				if (this.matAnim != value)
				{
					if (this.matAnim != null)
					{
						this.matAnim.PropertyChanged -= this.MatAnimPropertyChanged;
					}
					this.matAnim = value;
					if (this.matAnim != null)
					{
						this.matAnim.PropertyChanged += this.MatAnimPropertyChanged;
					}
					this.RaisePropertyChanged("MatAnim");
				}
			}
		}

		public bool MergeGeom
		{
			get
			{
				return this.mergeGeom;
			}
			set
			{
				if (this.mergeGeom != value)
				{
					this.mergeGeom = value;
					this.RaisePropertyChanged("MergeGeom");
				}
			}
		}

		public ModulateMode ModulateMode
		{
			get
			{
				return this.modulateMode;
			}
			set
			{
				if (this.modulateMode != value)
				{
					this.modulateMode = value;
					this.RaisePropertyChanged("ModulateMode");
				}
			}
		}

		public bool NoFog
		{
			get
			{
				return this.noFog;
			}
			set
			{
				if (this.noFog != value)
				{
					this.noFog = value;
					this.RaisePropertyChanged("NoFog");
				}
			}
		}

		public ShadeMode ShadeMode
		{
			get
			{
				return this.shadeMode;
			}
			set
			{
				if (this.shadeMode != value)
				{
					this.shadeMode = value;
					this.RaisePropertyChanged("ShadeMode");
				}
			}
		}

		public ResourceReference ShaderTechnique
		{
			get
			{
				return this.shaderTechnique;
			}
		}

		public int SpecularPower
		{
			get
			{
				return this.specularPower;
			}
			set
			{
				if (this.specularPower != value)
				{
					this.specularPower = value;
					this.RaisePropertyChanged("SpecularPower");
				}
			}
		}

		public ResourceReference Texture0
		{
			get
			{
				return this.texture0;
			}
		}

		public ResourceReference Texture1
		{
			get
			{
				return this.texture1;
			}
		}

		public ResourceReference Texture2
		{
			get
			{
				return this.texture2;
			}
		}

		public ResourceReference Texture3
		{
			get
			{
				return this.texture3;
			}
		}

		public string VertexShader
		{
			get
			{
				return this.vertexShader;
			}
			set
			{
				if (this.vertexShader != value)
				{
					this.vertexShader = value;
					this.RaisePropertyChanged("VertexShader");
				}
			}
		}

		public int ZDepthOfs
		{
			get
			{
				return this.zDepthOfs;
			}
			set
			{
				if (this.zDepthOfs != value)
				{
					this.zDepthOfs = value;
					this.RaisePropertyChanged("ZDepthOfs");
				}
			}
		}

		public int ZDepthOfsHW
		{
			get
			{
				return this.zDepthOfsHw;
			}
			set
			{
				if (this.zDepthOfsHw != value)
				{
					this.zDepthOfsHw = value;
					this.RaisePropertyChanged("ZDepthOfsHW");
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public void ApplyOpenGL()
		{
			GL.Color3(1.0f, 1.0f, 1.0f);

			switch (this.CullMode)
			{
				case CullMode.FRONT:
					if (!this.graphicsContext.FlipCulling)
					{
						GL.CullFace(CullFaceMode.Front);
					}
					else
					{
						GL.CullFace(CullFaceMode.Back);
					}
					GL.Enable(EnableCap.CullFace);
					break;
				case CullMode.BACK:
					if (!this.graphicsContext.FlipCulling)
					{
						GL.CullFace(CullFaceMode.Back);
					}
					else
					{
						GL.CullFace(CullFaceMode.Front);
					}
					GL.Enable(EnableCap.CullFace);
					break;
				case CullMode.NONE:
					GL.CullFace(CullFaceMode.FrontAndBack);
					GL.Disable(EnableCap.CullFace);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			switch (this.ShadeMode)
			{
				case ShadeMode.FLAT:
					GL.ShadeModel(ShadingModel.Flat);
					break;
				case ShadeMode.GOURAUD:
					GL.ShadeModel(ShadingModel.Smooth);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			switch (this.AlphaMode)
			{
				case AlphaMode.NONE:
					GL.Disable(EnableCap.Blend);
					break;
				case AlphaMode.HALF:
					break;
				case AlphaMode.ADD:
					GL.Enable(EnableCap.Blend);
					GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
					break;
				case AlphaMode.SUB:
					break;
				case AlphaMode.BLEND:
					GL.Enable(EnableCap.Blend);
					GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
					break;
				case AlphaMode.DEFAULT:
					GL.Disable(EnableCap.Blend);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			switch (this.AlphaTestMode)
			{
				case AlphaTestMode.DISABLED:
					GL.Disable(EnableCap.AlphaTest);
					break;
				case AlphaTestMode.NEVER:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Never, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.LESS:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Less, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.EQUAL:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Equal, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.LEQUAL:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Lequal, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.GREATER:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Greater, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.NOTEQUAL:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Notequal, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.GEQUAL:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Gequal, this.alphaTestValue / 255.0f);
					break;
				case AlphaTestMode.ALWAYS:
					GL.Enable(EnableCap.AlphaTest);
					GL.AlphaFunc(AlphaFunction.Always, this.alphaTestValue / 255.0f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			GL.DepthMask(this.DepthWriteEnable);

			GL.Enable(EnableCap.ColorMaterial);

			//GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat_specular);
			//GL.Material(MaterialFace.Front, MaterialParameter.Shininess, mat_shininess);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, this.ColDiffuse);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, this.ColEmissive);
			GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, this.ColSpecular);

			var technique = this.shaderTechnique.Resource as ShaderTechnique;
			if (this.graphicsContext.IsShadersEnabled && technique != null)
			{
				this.ApplyTextureToChannel(this.texture0, 0);
				this.ApplyTextureToChannel(this.texture1, 1);
				this.ApplyTextureToChannel(this.texture2, 2);
				this.ApplyTextureToChannel(this.texture3, 3);

				technique.ApplyOpenGL();
			}
			else
			{
				switch (this.EffectPreset)
				{
					case EffectPreset.DEFAULT:
						this.ApplyDefaultEffect();
						break;
					case EffectPreset.NORMAL_MAPPING:
						this.ApplyNormalMapEffect();
						break;
					case EffectPreset.REFLECTION_MAPPING:
						this.ApplyTexture0Effect();
						break;
					case EffectPreset.ENVIRONMENT_MAPPING:
						this.ApplyTexture0Effect();
						break;
					case EffectPreset.CONSTANT_COLOUR_CHANNEL:
						this.ApplyTexture0Effect();
						break;
					case EffectPreset.LIGHTMAP_POST_PROCESS:
						this.ApplyTexture0Effect();
						break;
					case EffectPreset.NORMAL_MAPPING_SPECULAR:
						this.ApplyTexture0Effect();
						break;
					case EffectPreset.TEXTURE0_ONLY:
						this.ApplyTexture0Effect();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public uint Flags
		{
			set
			{
				if (0 != (value & INTENSITY_F))
				{
				}
				else
				{
					
				}
				if (0 != (value & UNMODULATE_F))
				{
				}
				else
				{
					
				}
				if (0 != (value & FLAT_F))
				{
					this.ShadeMode = ShadeMode.FLAT;
				}
				else
				{
					this.ShadeMode = ShadeMode.GOURAUD;
				}
				if (0 != (value & TWO_SIDED_F))
				{
					this.CullMode = CullMode.NONE;
				}
				else if (0 != (value & CULL_FRONT_F))
				{
					this.CullMode = CullMode.FRONT;
				}
				else
				{
					this.CullMode = CullMode.BACK;
				}
				Filtering =  (0 == (value & NO_FILTERING_F));
				Invisible =  (0 != (value & NO_RENDER_F));
				MergeGeom =  (0 != (value & MERGE_GEOM_F));
				ClampUV =  (0 != (value & CLAMP_UV_F));
				AtlasMaterial =  (0 != (value & ATLAS_MATERIAL_F));
				NoFog =  (0 != (value & NO_FOG_F));

				AlphaMode = (AlphaMode) ((value & ALPHA_MODE_MASK)>>ALPHA_MODE_SHIFT);
				BlendMode = (BlendMode) ((value & BLEND_MODE_MASK)>>BLEND_MODE_SHIFT);
				EffectPreset = (EffectPreset)((value & EFFECT_PRESET_MASK) >> EFFECT_PRESET_SHIFT);
				AlphaTestMode = (AlphaTestMode)((value & ALPHATEST_MODE_MASK) >> ALPHATEST_MODE_SHIFT);
				DepthWriteEnable = 0==((value & DEPTH_WRITE_MODE_MASK) >> DEPTH_WRITE_MODE_SHIFT);
			}
		}

		protected bool AtlasMaterial
		{
			get
			{
				return atlasMaterial;
			}
			set
			{
				if (atlasMaterial != value)
				{
					this.RaisePropertyChanging("AtlasMaterial");
					atlasMaterial = value;
					this.RaisePropertyChanged("AtlasMaterial");
				}
			}
		}

		#endregion

		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.texture0.Dispose();
			}
		}

		private void ApplyDefaultEffect()
		{
			this.ApplyTextureToChannel(this.texture0, 0);
			this.ApplyTextureToChannel(this.texture1, 1);
			GL.ActiveTexture(TextureUnit.Texture1);
			switch (this.BlendMode)
			{
				case BlendMode.MODULATE:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
					break;
				case BlendMode.DECAL:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Decal);
					break;
				case BlendMode.ADD:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Add);
					break;
				case BlendMode.REPLACE:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);
					break;
				case BlendMode.BLEND:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Blend);
					break;
				case BlendMode.MODULATE_2X:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
					break;
				case BlendMode.MODULATE_4X:
					GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ApplyNormalMapEffect()
		{
			this.ApplyTexture0Effect();
			//this.ApplyTextureToChannel(this.texture1, 0);
			//GL.ActiveTexture(TextureUnit.Texture0);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_COMBINE_RGB, GL_DOT3_RGB);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE0_RGB, GL_PREVIOUS);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE1_RGB, GL_TEXTURE);

			//this.ApplyTextureToChannel(this.texture0, 1);
			//GL.ActiveTexture(TextureUnit.Texture1);

			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_COMBINE);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_COMBINE_RGB, GL_DOT3_RGB);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE0_RGB, GL_PREVIOUS);
			//GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE1_RGB, GL_TEXTURE);
		}

		private void ApplyTexture0Effect()
		{
			this.ApplyTextureToChannel(this.texture0, 0);
		}

		private void ApplyTextureToChannel(ResourceReference resourceReference, int i)
		{
			var resource = resourceReference.Resource as Texture;
			if (resource != null)
			{
				resource.ApplyOpenGL(i);
				this.SetFiltering(i);
			}
			else
			{
				GL.ActiveTexture(TextureUnit.Texture0 + i);
				GL.Disable(EnableCap.Texture2D);
			}
		}

		private void MatAnimPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RaisePropertyChanged("MatAnim");
		}

		private void SetFiltering(int i)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + i);
			if (this.filtering)
			{
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}
			else
			{
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}
		}

		#endregion
	}
}
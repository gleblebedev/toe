using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;

using OpenTK.Graphics.OpenGL;

using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.IwGx
{
	public class Material : Managed
	{
		#region Constants and Fields

		public const uint ALPHATEST_MODE_MASK = 0x1e000000; // Alpha Test member

		public const int ALPHATEST_MODE_SHIFT = 25;

		public const uint ALPHA_MODE_MASK = 0x00070000; // AlphaMode member

		public const int ALPHA_MODE_SHIFT = 16;

		public const uint ATLAS_MATERIAL_F = (1 << 9);

		public const uint BLEND_MODE_MASK = 0x00380000; // BlendMode member

		public const int BLEND_MODE_SHIFT = 19;

		public const uint CLAMP_UV_F = (1 << 8); // If set the texture coords are clamped when the texture object is set

		public const uint CULL_FRONT_F = (1 << 4);
		                  // if TWO_SIDED_F is NOT set; cull front-facing rather than back-facing polys

		/// <summary>
		/// Depth test mode member
		/// </summary>
		public const uint DEPTH_WRITE_MODE_MASK = 0x1 << DEPTH_WRITE_MODE_SHIFT;

		public const int DEPTH_WRITE_MODE_SHIFT = 29;

		public const uint EFFECT_PRESET_MASK = 0x01C00000; // EffectPreset member

		public const int EFFECT_PRESET_SHIFT = 22;

		public const uint FLAT_F = (1 << 2); // do NOT use gouraud shading for this material

		/// <summary>
		/// if gouraud-shaded, perform intensity shading only, using R component of colour
		/// </summary>
		public const uint INTENSITY_F = (1 << 0);

		public const uint IN_USE_F = (1 << 12); // material is being used to render this frame (used when caching geometry)

		public const uint MERGE_GEOM_F = (1 << 7); // If trans=SW; light=SW; rast=HW: try to merge all MatGeomInfo into one

		public const uint NO_FILTERING_F = (1 << 5); // disable filtering for all textures

		public const uint NO_FOG_F = (1 << 10); // Disable Fogging for this material

		public const uint NO_RENDER_F = (1 << 6); // do not render geometry with this material

		public const uint PRIVATE_FLAGS_MASK = 0xffffffff;

		public const uint TWO_SIDED_F = (1 << 3); // material is two-sided (perform no culling)

		/// <summary>
		/// No shading or lighting, fully lit
		/// </summary>
		public const uint UNMODULATE_F = (1 << 1);

		public static readonly uint TypeHash = Hash.Get("CIwMaterial");

		private readonly IResourceManager resourceManager;

		private readonly ResourceReference shaderTechnique;

		private readonly ResourceReference texture0;

		private readonly ResourceReference texture1;

		private readonly ResourceReference texture2;

		private readonly ResourceReference texture3;

		private AlphaMode alphaMode = AlphaMode.DEFAULT;

		private AlphaTestMode alphaTestMode = AlphaTestMode.DISABLED;

		private byte alphaTestValue;

		private bool atlasMaterial;

		private BlendMode blendMode = BlendMode.MODULATE;

		private bool clampUV;

		private Color colAmbient = Color.FromArgb(255, 255, 255, 255);

		private Color colDiffuse = Color.FromArgb(255, 255, 255, 255);

		private Color colEmissive = Color.FromArgb(0, 0, 0, 0);

		private Color colSpecular = Color.FromArgb(10, 0, 0, 0);

		private CullMode cullMode = CullMode.BACK;

		private bool depthWriteEnable = true;

		private EffectPreset effectPreset = EffectPreset.DEFAULT;

		private ImageFormat formatHw;

		private ImageFormat formatSw;

		private bool invisible;

		private bool keepAfterUpload;

		private MatAnim matAnim;

		private bool mergeGeom;

		private ModulateMode modulateMode = ModulateMode.RGB;

		private bool noFog;

		private ShadeMode shadeMode = ShadeMode.GOURAUD;

		private string vertexShader;

		private int zDepthOfs;

		private int zDepthOfsHw;

		#endregion

		#region Constructors and Destructors

		public Material(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			this.texture0 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture0.ReferenceChanged += (s, a) => this.RaisePropertyChanged(Texture0EventArgs.Changed);

			this.texture1 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture1.ReferenceChanged += (s, a) => this.RaisePropertyChanged(Texture1EventArgs.Changed);

			this.texture2 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture2.ReferenceChanged += (s, a) => this.RaisePropertyChanged(Texture2EventArgs.Changed);

			this.texture3 = new ResourceReference(Texture.TypeHash, resourceManager, this);
			this.texture3.ReferenceChanged += (s, a) => this.RaisePropertyChanged(Texture3EventArgs.Changed);

			this.shaderTechnique = new ResourceReference(IwGx.ShaderTechnique.TypeHash, resourceManager, this);
			this.shaderTechnique.ReferenceChanged += (s, a) => this.RaisePropertyChanged(ShaderTechniqueEventArgs.Changed);
		}

		#endregion

		#region Public Properties

		protected static PropertyEventArgs AlphaModeEventArgs = Expr.PropertyEventArgs<Material>(x => x.AlphaMode);


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
					this.RaisePropertyChanging(AlphaModeEventArgs.Changing);
					this.alphaMode = value;
					this.RaisePropertyChanged(AlphaModeEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs AlphaTestModeEventArgs = Expr.PropertyEventArgs<Material>(x => x.AlphaTestMode);

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
					this.RaisePropertyChanging(AlphaTestModeEventArgs.Changing);
					this.alphaTestMode = value;
					this.RaisePropertyChanged(AlphaTestModeEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs AlphaTestValueEventArgs = Expr.PropertyEventArgs<Material>(x => x.AlphaTestValue);

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
					this.RaisePropertyChanging(AlphaTestValueEventArgs.Changing);
					this.alphaTestValue = value;
					this.RaisePropertyChanged(AlphaTestValueEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs BlendModeEventArgs = Expr.PropertyEventArgs<Material>(x => x.BlendMode);

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
					this.RaisePropertyChanging(BlendModeEventArgs.Changing);
					this.blendMode = value;
					this.RaisePropertyChanged(BlendModeEventArgs.Changed);
				}
			}
		}
		

		protected static PropertyEventArgs ClampUVEventArgs = Expr.PropertyEventArgs<Material>(x => x.ClampUV);


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
					this.RaisePropertyChanging(ClampUVEventArgs.Changing);
					this.clampUV = value;
					this.RaisePropertyChanged(ClampUVEventArgs.Changed);
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

		protected static PropertyEventArgs ColAmbientEventArgs = Expr.PropertyEventArgs<Material>(x => x.ColAmbient);

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
					this.RaisePropertyChanging(ColAmbientEventArgs.Changing);
					this.colAmbient = value;
					this.RaisePropertyChanged(ColAmbientEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ColDiffuseEventArgs = Expr.PropertyEventArgs<Material>(x => x.ColDiffuse);


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
					this.RaisePropertyChanging(ColDiffuseEventArgs.Changing);
					this.colDiffuse = value;
					this.RaisePropertyChanged(ColDiffuseEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ColEmissiveEventArgs = Expr.PropertyEventArgs<Material>(x => x.ColEmissive);

		
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
					this.RaisePropertyChanging(ColEmissiveEventArgs.Changing);
					this.colEmissive = value;
					this.RaisePropertyChanged(ColEmissiveEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ColSpecularEventArgs = Expr.PropertyEventArgs<Material>(x => x.ColSpecular);

	
		public Color ColSpecular
		{
			get
			{
				return Color.FromArgb(255, this.colSpecular.R, this.colSpecular.G, this.colSpecular.B);
			}
			set
			{
				if (this.colSpecular != value)
				{
					this.RaisePropertyChanging(ColSpecularEventArgs.Changing);
					this.colSpecular = Color.FromArgb(this.SpecularPower, value.R, value.G, value.B);
					this.RaisePropertyChanged(ColSpecularEventArgs.Changed);
				}
			}
		}


		protected static PropertyEventArgs CullModeEventArgs = Expr.PropertyEventArgs<Material>(x => x.CullMode);

	
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
					this.RaisePropertyChanging(CullModeEventArgs.Changing);
					this.cullMode = value;
					this.RaisePropertyChanged(CullModeEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs DepthWriteEnableEventArgs = Expr.PropertyEventArgs<Material>(x => x.DepthWriteEnable);



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
					this.RaisePropertyChanging(DepthWriteEnableEventArgs.Changing);
					this.depthWriteEnable = value;
					this.RaisePropertyChanged(DepthWriteEnableEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs EffectPresetEventArgs = Expr.PropertyEventArgs<Material>(x => x.EffectPreset);

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
					this.RaisePropertyChanging(EffectPresetEventArgs.Changing);
					this.effectPreset = value;
					this.RaisePropertyChanged(EffectPresetEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs FilteringEventArgs = Expr.PropertyEventArgs<Material>(x => x.Filtering);

		private bool filtering;

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
					this.RaisePropertyChanging(FilteringEventArgs.Changing);
					this.filtering = value;
					this.RaisePropertyChanged(FilteringEventArgs.Changed);
				}
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
				this.Filtering = (0 == (value & NO_FILTERING_F));
				this.Invisible = (0 != (value & NO_RENDER_F));
				this.MergeGeom = (0 != (value & MERGE_GEOM_F));
				this.ClampUV = (0 != (value & CLAMP_UV_F));
				this.AtlasMaterial = (0 != (value & ATLAS_MATERIAL_F));
				this.NoFog = (0 != (value & NO_FOG_F));

				this.AlphaMode = (AlphaMode)((value & ALPHA_MODE_MASK) >> ALPHA_MODE_SHIFT);
				this.BlendMode = (BlendMode)((value & BLEND_MODE_MASK) >> BLEND_MODE_SHIFT);
				this.EffectPreset = (EffectPreset)((value & EFFECT_PRESET_MASK) >> EFFECT_PRESET_SHIFT);
				this.AlphaTestMode = (AlphaTestMode)((value & ALPHATEST_MODE_MASK) >> ALPHATEST_MODE_SHIFT);
				this.DepthWriteEnable = 0 == ((value & DEPTH_WRITE_MODE_MASK) >> DEPTH_WRITE_MODE_SHIFT);
			}
		}

		protected static PropertyEventArgs FormatHWEventArgs = Expr.PropertyEventArgs<Material>(x => x.FormatHW);


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
					this.RaisePropertyChanging(FormatHWEventArgs.Changing);
					this.formatHw = value;
					this.RaisePropertyChanged(FormatHWEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs FormatSWEventArgs = Expr.PropertyEventArgs<Material>(x => x.FormatSW);

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
					this.RaisePropertyChanging(FormatSWEventArgs.Changing);
					this.formatSw = value;
					this.RaisePropertyChanged(FormatSWEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs InvisibleEventArgs = Expr.PropertyEventArgs<Material>(x => x.Invisible);

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
					this.RaisePropertyChanging(InvisibleEventArgs.Changing);
					this.invisible = value;
					this.RaisePropertyChanged(InvisibleEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs KeepAfterUploadEventArgs = Expr.PropertyEventArgs<Material>(x => x.KeepAfterUpload);

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
					this.RaisePropertyChanging(KeepAfterUploadEventArgs.Changing);
					this.keepAfterUpload = value;
					this.RaisePropertyChanged(KeepAfterUploadEventArgs.Changed);
				}
			}
		}
		protected static PropertyEventArgs MatAnimEventArgs = Expr.PropertyEventArgs<Material>(x => x.MatAnim);


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
					this.RaisePropertyChanging(MatAnimEventArgs.Changing);
					this.matAnim = value;
					this.RaisePropertyChanged(MatAnimEventArgs.Changed);
				}
			}
		}


		protected static PropertyEventArgs MergeGeomEventArgs = Expr.PropertyEventArgs<Material>(x => x.MergeGeom);


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
					this.RaisePropertyChanging(MergeGeomEventArgs.Changing);
					this.mergeGeom = value;
					this.RaisePropertyChanged(MergeGeomEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ModulateModeEventArgs = Expr.PropertyEventArgs<Material>(x => x.ModulateMode);

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
					this.RaisePropertyChanging(ModulateModeEventArgs.Changing);
					this.modulateMode = value;
					this.RaisePropertyChanged(ModulateModeEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs NoFogEventArgs = Expr.PropertyEventArgs<Material>(x => x.NoFog);


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
					this.RaisePropertyChanging(NoFogEventArgs.Changing);
					this.noFog = value;
					this.RaisePropertyChanged(NoFogEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ShadeModeEventArgs = Expr.PropertyEventArgs<Material>(x => x.ShadeMode);

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
					this.RaisePropertyChanging(ShadeModeEventArgs.Changing);
					this.shadeMode = value;
					this.RaisePropertyChanged(ShadeModeEventArgs.Changed);
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

		public Color SpecularCombined
		{
			get
			{
				return this.colSpecular;
			}
		}

		protected static PropertyEventArgs SpecularPowerEventArgs = Expr.PropertyEventArgs<Material>(x => x.SpecularPower);

		public byte SpecularPower
		{
			get
			{
				return this.colSpecular.A;
			}
			set
			{
				if (this.SpecularPower != value)
				{
					this.RaisePropertyChanging(SpecularPowerEventArgs.Changing);
					this.RaisePropertyChanging(ColSpecularEventArgs.Changing);
					this.colSpecular = Color.FromArgb(value, this.colSpecular.R, this.colSpecular.G, this.colSpecular.B);
					this.RaisePropertyChanged(SpecularPowerEventArgs.Changed);
					this.RaisePropertyChanged(ColSpecularEventArgs.Changed);
				}
			}
		}
	

		protected static PropertyEventArgs Texture0EventArgs = Expr.PropertyEventArgs<Material>(x => x.Texture0);
		protected static PropertyEventArgs Texture1EventArgs = Expr.PropertyEventArgs<Material>(x => x.Texture1);
		protected static PropertyEventArgs Texture2EventArgs = Expr.PropertyEventArgs<Material>(x => x.Texture2);
		protected static PropertyEventArgs Texture3EventArgs = Expr.PropertyEventArgs<Material>(x => x.Texture3);
		protected static PropertyEventArgs ShaderTechniqueEventArgs = Expr.PropertyEventArgs<Material>(x => x.ShaderTechnique);

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

		protected static PropertyEventArgs VertexShaderEventArgs = Expr.PropertyEventArgs<Material>(x => x.VertexShader);

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
					this.RaisePropertyChanging(VertexShaderEventArgs.Changing);
					this.vertexShader = value;
					this.RaisePropertyChanged(VertexShaderEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ZDepthOfsEventArgs = Expr.PropertyEventArgs<Material>(x => x.ZDepthOfs);

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
					this.RaisePropertyChanging(ZDepthOfsEventArgs.Changing);
					this.zDepthOfs = value;
					this.RaisePropertyChanged(ZDepthOfsEventArgs.Changed);
				}
			}
		}

		protected static PropertyEventArgs ZDepthOfsHWEventArgs = Expr.PropertyEventArgs<Material>(x => x.ZDepthOfsHW);

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
					this.RaisePropertyChanging(ZDepthOfsHWEventArgs.Changing);
					this.zDepthOfsHw = value;
					this.RaisePropertyChanged(ZDepthOfsHWEventArgs.Changed);
				}
			}
		}

		#endregion

		#region Properties

		protected static PropertyEventArgs AtlasMaterialEventArgs = Expr.PropertyEventArgs<Material>(x => x.AtlasMaterial);

		public bool AtlasMaterial
		{
			get
			{
				return this.atlasMaterial;
			}
			set
			{
				if (this.atlasMaterial != value)
				{
					this.RaisePropertyChanging(AtlasMaterialEventArgs.Changing);
					this.atlasMaterial = value;
					this.RaisePropertyChanged(AtlasMaterialEventArgs.Changed);
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		//public void ApplyOpenGL()
		//{
		//    GL.Color3(1.0f, 1.0f, 1.0f);

		//    switch (this.CullMode)
		//    {
		//        case CullMode.FRONT:
		//            if (!this.graphicsContext.FlipCulling)
		//            {
		//                GL.CullFace(CullFaceMode.Front);
		//            }
		//            else
		//            {
		//                GL.CullFace(CullFaceMode.Back);
		//            }
		//            GL.Enable(EnableCap.CullFace);
		//            break;
		//        case CullMode.BACK:
		//            if (!this.graphicsContext.FlipCulling)
		//            {
		//                GL.CullFace(CullFaceMode.Back);
		//            }
		//            else
		//            {
		//                GL.CullFace(CullFaceMode.Front);
		//            }
		//            GL.Enable(EnableCap.CullFace);
		//            break;
		//        case CullMode.NONE:
		//            GL.CullFace(CullFaceMode.FrontAndBack);
		//            GL.Disable(EnableCap.CullFace);
		//            break;
		//        default:
		//            throw new ArgumentOutOfRangeException();
		//    }
		//    switch (this.ShadeMode)
		//    {
		//        case ShadeMode.FLAT:
		//            GL.ShadeModel(ShadingModel.Flat);
		//            break;
		//        case ShadeMode.GOURAUD:
		//            GL.ShadeModel(ShadingModel.Smooth);
		//            break;
		//        default:
		//            throw new ArgumentOutOfRangeException();
		//    }
		//    switch (this.AlphaMode)
		//    {
		//        case AlphaMode.NONE:
		//            GL.Disable(EnableCap.Blend);
		//            break;
		//        case AlphaMode.HALF:
		//            break;
		//        case AlphaMode.ADD:
		//            GL.Enable(EnableCap.Blend);
		//            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
		//            break;
		//        case AlphaMode.SUB:
		//            break;
		//        case AlphaMode.BLEND:
		//            GL.Enable(EnableCap.Blend);
		//            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		//            break;
		//        case AlphaMode.DEFAULT:
		//            GL.Disable(EnableCap.Blend);
		//            break;
		//        default:
		//            throw new ArgumentOutOfRangeException();
		//    }
		//    switch (this.AlphaTestMode)
		//    {
		//        case AlphaTestMode.DISABLED:
		//            GL.Disable(EnableCap.AlphaTest);
		//            break;
		//        case AlphaTestMode.NEVER:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Never, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.LESS:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Less, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.EQUAL:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Equal, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.LEQUAL:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Lequal, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.GREATER:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Greater, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.NOTEQUAL:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Notequal, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.GEQUAL:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Gequal, this.alphaTestValue / 255.0f);
		//            break;
		//        case AlphaTestMode.ALWAYS:
		//            GL.Enable(EnableCap.AlphaTest);
		//            GL.AlphaFunc(AlphaFunction.Always, this.alphaTestValue / 255.0f);
		//            break;
		//        default:
		//            throw new ArgumentOutOfRangeException();
		//    }
		//    GL.DepthMask(this.DepthWriteEnable);

		//    GL.Enable(EnableCap.ColorMaterial);

		//    if (this.colSpecular.A > 0 && (this.colSpecular.R > 0 || this.colSpecular.G > 0 || this.colSpecular.B > 0))
		//    {
		//        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, this.ColSpecular);
		//        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, this.SpecularPower / 255.0f);
		//    }
		//    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, this.ColDiffuse);
		//    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, this.ColEmissive);
		//    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, this.ColAmbient);

		//    var technique = this.shaderTechnique.Resource as ShaderTechnique;
		//    if (this.graphicsContext.IsShadersEnabled && technique != null)
		//    {
		//        this.ApplyTextureToChannel(this.texture0, 0);
		//        this.ApplyTextureToChannel(this.texture1, 1);
		//        this.ApplyTextureToChannel(this.texture2, 2);
		//        this.ApplyTextureToChannel(this.texture3, 3);

		//        technique.ApplyOpenGL();
		//    }
		//    else
		//    {
		//        switch (this.EffectPreset)
		//        {
		//            case EffectPreset.DEFAULT:
		//                this.ApplyDefaultEffect();
		//                break;
		//            case EffectPreset.NORMAL_MAPPING:
		//                this.ApplyNormalMapEffect();
		//                break;
		//            case EffectPreset.REFLECTION_MAPPING:
		//                this.ApplyTexture0Effect();
		//                break;
		//            case EffectPreset.ENVIRONMENT_MAPPING:
		//                this.ApplyTexture0Effect();
		//                break;
		//            case EffectPreset.CONSTANT_COLOUR_CHANNEL:
		//                this.ApplyTexture0Effect();
		//                break;
		//            case EffectPreset.LIGHTMAP_POST_PROCESS:
		//                this.ApplyTexture0Effect();
		//                break;
		//            case EffectPreset.NORMAL_MAPPING_SPECULAR:
		//                this.ApplyTexture0Effect();
		//                break;
		//            case EffectPreset.TEXTURE0_ONLY:
		//                this.ApplyTexture0Effect();
		//                break;
		//            default:
		//                throw new ArgumentOutOfRangeException();
		//        }
		//    }
		//}

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

		//private void ApplyDefaultEffect()
		//{
		//    this.ApplyTextureToChannel(this.texture0, 0);
		//    this.ApplyTextureToChannel(this.texture1, 1);
		//    GL.ActiveTexture(TextureUnit.Texture1);
		//    switch (this.BlendMode)
		//    {
		//        case BlendMode.MODULATE:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
		//            break;
		//        case BlendMode.DECAL:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Decal);
		//            break;
		//        case BlendMode.ADD:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Add);
		//            break;
		//        case BlendMode.REPLACE:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);
		//            break;
		//        case BlendMode.BLEND:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Blend);
		//            break;
		//        case BlendMode.MODULATE_2X:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
		//            break;
		//        case BlendMode.MODULATE_4X:
		//            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
		//            break;
		//        default:
		//            throw new ArgumentOutOfRangeException();
		//    }
		//}

		//private void ApplyNormalMapEffect()
		//{
		//    this.ApplyTexture0Effect();
		//    //this.ApplyTextureToChannel(this.texture1, 0);
		//    //GL.ActiveTexture(TextureUnit.Texture0);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_COMBINE_RGB, GL_DOT3_RGB);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE0_RGB, GL_PREVIOUS);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE1_RGB, GL_TEXTURE);

		//    //this.ApplyTextureToChannel(this.texture0, 1);
		//    //GL.ActiveTexture(TextureUnit.Texture1);

		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_COMBINE);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_COMBINE_RGB, GL_DOT3_RGB);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE0_RGB, GL_PREVIOUS);
		//    //GL.TexEnv(TextureEnvTarget.TextureEnv, GL_TEXTURE_ENV, GL_SOURCE1_RGB, GL_TEXTURE);
		//}

		//private void ApplyTexture0Effect()
		//{
		//    this.ApplyTextureToChannel(this.texture0, 0);
		//}

		//private void ApplyTextureToChannel(ResourceReference resourceReference, int i)
		//{
		//    var resource = resourceReference.Resource as Texture;
		//    if (resource != null)
		//    {
		//        resource.ApplyOpenGL(i);
		//        this.SetFiltering(i);
		//    }
		//    else
		//    {
		//        GL.ActiveTexture(TextureUnit.Texture0 + i);
		//        GL.Disable(EnableCap.Texture2D);
		//    }
		//}

		private void MatAnimPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RaisePropertyChanged(MatAnimEventArgs.Changed);
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
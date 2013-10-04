using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Marmalade.IwGraphics;
using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Mesh;

using CullMode = Toe.Utils.Marmalade.IwGx.CullMode;
using Image = Toe.Marmalade.IwGx.Image;

namespace Toe.Gx
{
	public class ToeGraphicsContext : IDisposable
	{
		#region Constants and Fields

		private readonly ShaderTechniqueArguments args = new ShaderTechniqueArguments();

		private readonly ToeGxIndexBufferItem[] indexBuffer = new ToeGxIndexBufferItem[1024];

		private readonly bool[] isTextureSet = new bool[4];

		private readonly IResourceManager resourceManager;

		private readonly DefaultShaders shaders = new DefaultShaders();

		private readonly Vertex[] vertexBuffer = new Vertex[1024 * 64];

		private readonly NativeWindow window;

		private GraphicsContext context;

		private Material debugMaterial;

		private bool flipCulling = true;

		private Frustum frustum;

		private int indexBufferCount;

		private LightArgs light;

		private bool lighting;

		private Material material;

		private Matrix4 model = Matrix4.Identity;

		private Matrix4 modelView = Matrix4.Identity;

		private Matrix4 projection = Matrix4.Identity;

		private Thread resourceContextThread;

		private int vertexBufferCount;

		private Matrix4 view = Matrix4.Identity;

		private IVsdProvider vsdProvider;

		#endregion

		#region Constructors and Destructors

		public ToeGraphicsContext(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			this.debugMaterial = new Material(this.resourceManager)
				{ ColEmissive = Color.FromArgb(255, 255, 255, 255), SpecularPower = 0 };
			GraphicsContext.ShareContexts = true;
			var currentContext = GraphicsContext.CurrentContext;
			if (currentContext != null)
			{
				currentContext.MakeCurrent(null);
			}
			//EventWaitHandle context_ready = new EventWaitHandle(false, EventResetMode.AutoReset);

			//this.resourceContextThread = new Thread(
			//	() =>
			//		{
			this.window = new NativeWindow();
			GraphicsContext.ShareContexts = true;
			this.context = new GraphicsContext(GraphicsMode.Default, this.window.WindowInfo);
			this.context.MakeCurrent(this.window.WindowInfo);

			this.window.ProcessEvents();
			//context_ready.Set();
			//while (this.window.Exists)
			//{
			//    this.window.ProcessEvents();

			//    // Perform your processing here

			//    Thread.Sleep(1); // Limit CPU usage, if necessary
			//}
			//});
			//this.resourceContextThread.IsBackground = true;
			//this.resourceContextThread.Start();

			//context_ready.WaitOne();
		}

		~ToeGraphicsContext()
		{
			this.Dispose(false);
		}

		#endregion

		#region Public Properties

		public bool FlipCulling
		{
			get
			{
				return this.flipCulling;
			}
			set
			{
				this.flipCulling = value;
			}
		}

		public bool IsShadersEnabled { get; set; }

		public IVsdProvider VsdProvider
		{
			get
			{
				return this.vsdProvider;
			}
			set
			{
				if (this.vsdProvider != value)
				{
					this.vsdProvider = value;
					this.UpdateFrustum();
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public void DisableLighting()
		{
			GL.Disable(EnableCap.Lighting);
			this.lighting = false;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void EnableLighting()
		{
			GL.Enable(EnableCap.Lighting);
			this.lighting = true;
		}

		public void Flush()
		{
			this.RenderVertexBuffer();
			OpenTKHelper.Assert();
			GL.Flush();
			OpenTKHelper.Assert();
		}

		public void ModelToWorld(ref Vector3 vec, out Vector3 r)
		{
			Vector3.Transform(ref vec, ref this.model, out r);
		}

		public void ModelToWorldView(ref Vector3 vec, out Vector3 r)
		{
			Vector3.Transform(ref vec, ref this.modelView, out r);
		}

		public void Render(IMesh mesh)
		{
			if (mesh == null)
			{
				return;
			}

			if (!this.frustum.CheckSphere(mesh.BoundingSphereCenter, mesh.BoundingSphereR))
			{
				return;
			}

			var vertexBufferRenderData = mesh.RenderData as VertexBufferRenderData;
			if (vertexBufferRenderData == null)
			{
				mesh.RenderData = vertexBufferRenderData = new VertexBufferRenderData(mesh);
			}

			IEnumerable<ISubMesh> submeshes;
			if (this.vsdProvider != null && this.vsdProvider.Level == mesh)
			{
				submeshes = this.vsdProvider.GetVisibleSubMeshes();
			}
			else
			{
				submeshes = mesh.Submeshes;
			}
			foreach (var surface in submeshes)
			{
				//if (!this.frustum.CheckSphere(surface.BoundingSphereCenter, surface.BoundingSphereR))
				//{
				//    continue;
				//}

				this.SetMaterial(surface.Material);

				var p = this.ApplyMaterialShader(mesh);
				if (p == null)
				{
					return;
				}
				vertexBufferRenderData.Enable(p);

				this.Render(mesh, surface);
			}
		}

		public void RenderDebugLine(Vector3 from, Vector3 to, Color color)
		{
			this.RenderDebugLine(ref from, ref to, ref color);
		}

		public void RenderDebugLine(ref Vector3 from, ref Vector3 to, ref Color color)
		{
			if (this.vertexBufferCount + 2 > this.vertexBuffer.Length)
			{
				this.DropVertextBuffer();
			}
			this.vertexBuffer[this.vertexBufferCount] = new Vertex { Position = from, Color = color };
			++this.vertexBufferCount;
			this.vertexBuffer[this.vertexBufferCount] = new Vertex { Position = to, Color = color };
			++this.vertexBufferCount;
		}

		public void RenderModel(Model model)
		{
			foreach (var mesh in model.Meshes)
			{
				this.RenderMesh(mesh);
			}
		}

		public void SetLight0(ref LightArgs args)
		{
			this.light = args;
			if (!this.light.Enabled)
			{
				GL.Disable(EnableCap.Light0);
			}
			else
			{
				GL.Enable(EnableCap.Light0);
				GL.Light(
					LightName.Light0,
					LightParameter.Position,
					new[] { this.light.Position.X, this.light.Position.Y, this.light.Position.Z, 1.0f });
			}
		}

		public void SetMaterial(Material m)
		{
			this.material = m;
			if (this.material == null)
			{
				GL.UseProgram(0);
				GL.ActiveTexture(TextureUnit.Texture1);
				GL.Disable(EnableCap.Texture2D);
				GL.ActiveTexture(TextureUnit.Texture0);
				GL.Disable(EnableCap.Texture2D);
			}
			this.ApplyMaterialCommonFixedPipeline();
		}

		public void SetModel(ref Matrix4 model)
		{
			this.model = model;
			this.modelView = this.view * this.model;
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref this.modelView);
		}

		public void SetProjection(ref Matrix4 projection)
		{
			this.RenderVertexBuffer();
			this.projection = projection;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);
			GL.MatrixMode(MatrixMode.Modelview);
			this.UpdateFrustum();
		}

		public void SetTexture(int channel, Texture texture)
		{
			if (texture == null)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + channel);
				GL.Disable(EnableCap.Texture2D);
				this.isTextureSet[channel] = false;
				return;
			}
			this.isTextureSet[channel] = true;
			var textureContext = texture.ContextData as TextureContext;
			if (textureContext == null)
			{
				textureContext = new TextureContext(texture);
				texture.ContextData = textureContext;
			}
			textureContext.ApplyToChannel(channel);
		}

		public void SetView(ref Matrix4 view)
		{
			this.RenderVertexBuffer();
			this.view = view;
			this.modelView = view * this.model;
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref this.modelView);
			this.UpdateFrustum();
		}

		public void SetViewport(int x, int y, int w, int h)
		{
			this.RenderVertexBuffer();
			GL.Viewport(x, y, w, h);
			this.args.inDisplaySize = new Vector2(w, h);
			this.args.inDeviceSize = new Vector2(w, h);
		}

		#endregion

		#region Methods

		protected virtual void Dispose(bool disposing)
		{
			if (this.resourceContextThread != null)
			{
				this.resourceContextThread.Abort();
				this.resourceContextThread = null;
			}

			if (disposing)
			{
				if (this.context != null)
				{
					this.context.Dispose();
					this.context = null;
				}
			}
		}

		private static float[] BuildMat3x3(ref Matrix4 inModelRotMat)
		{
			return new[]
				{
					inModelRotMat.M11, inModelRotMat.M12, inModelRotMat.M13, inModelRotMat.M21, inModelRotMat.M22, inModelRotMat.M23,
					inModelRotMat.M31, inModelRotMat.M32, inModelRotMat.M33
				};
		}

		private static Vector4 ColorToVector4(Color colEmissive)
		{
			return new Vector4(colEmissive.R / 255.0f, colEmissive.G / 255.0f, colEmissive.B / 255.0f, colEmissive.A / 255.0f);
		}

		private ShaderTechniqueArgumentIndices ApplyDefaultMaterial(IVertexStreamSource mesh)
		{
			var vso = new DefaultVertexShaderOptions
				{
					BITANGENT_STREAM = false,
					//mesh.IsBinormalStreamAvailable,
					COL_STREAM = mesh.HasStream(Streams.Color, 0),
					FAST_FOG = false,
					FOG = false,
					//!material.NoFog,
					LIGHT_AMBIENT = this.lighting && this.light.Enabled,
					LIGHT_DIFFUSE =
						this.lighting && this.light.Enabled
						&& this.IsNotDisabledColor(this.light.Diffuse),
					LIGHT_SPECULAR =
						this.lighting && this.light.Enabled
						&& this.IsNotDisabledColor(this.light.Specular),
					LIGHT_EMISSIVE = this.lighting && this.light.Enabled,
					NORM_STREAM = mesh.HasStream(Streams.Normal, 0),
					SKIN_MAJOR_BONE = false,
					SKINWEIGHT_STREAM = false,
					SKIN_NORMALS = false,
					TANGENT_STREAM = false,
					//mesh.IsTangentStreamAvailable,
					UV0_STREAM = mesh.HasStream(Streams.TexCoord, 0) && this.isTextureSet[0],
					UV1_STREAM = mesh.HasStream(Streams.TexCoord, 1) && this.isTextureSet[1]
				};

			var fso = new DefaultFragmentShaderOptions
				{
					COL_STREAM = mesh.HasStream(Streams.Color, 0),
					FAST_FOG = false,
					FOG = false,
					//!material.NoFog,
					LIGHT_AMBIENT = this.lighting && this.light.Enabled,
					LIGHT_DIFFUSE = this.lighting && this.light.Enabled && this.IsNotDisabledColor(this.light.Diffuse),
					LIGHT_SPECULAR = this.lighting && this.light.Enabled && this.IsNotDisabledColor(this.light.Specular),
					LIGHT_EMISSIVE = this.lighting && this.light.Enabled,
					UV0_STREAM = mesh.HasStream(Streams.TexCoord, 0),
					UV1_STREAM = mesh.HasStream(Streams.TexCoord,1),
					ALPHA_BLEND = (int)AlphaMode.DEFAULT,
					ALPHA_TEST = (int)AlphaTestMode.DISABLED,
					BLEND = (int)BlendMode.MODULATE,
					EFFECT_PRESET = (int)EffectPreset.DEFAULT,
					IW_GX_PLATFORM_TEGRA2 = false,
					TEX0 = this.isTextureSet[0],
					TEX1 = this.isTextureSet[1]
				};

			ShaderTechniqueArgumentIndices p = this.shaders.GetProgram(new DefaultProgramOptions(vso, fso));
			GL.UseProgram(p.ProgramId);
			OpenTKHelper.Assert();

			this.BindShaderArgs(ref p);

			return p;
		}

		private void ApplyFiltering()
		{
			if (this.material == null || this.material.Filtering)
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

		private void ApplyMaterialCommonFixedPipeline()
		{
			if (this.material != null)
			{
				switch (this.material.CullMode)
				{
					case CullMode.FRONT:
						GL.Enable(EnableCap.CullFace);
						GL.CullFace(CullFaceMode.Front);
						break;
					case CullMode.BACK:
						GL.Enable(EnableCap.CullFace);
						GL.CullFace(CullFaceMode.Back);
						break;
					case CullMode.NONE:
						GL.Disable(EnableCap.CullFace);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				GL.Disable(EnableCap.CullFace);
				GL.ShadeModel(ShadingModel.Smooth);
			}
		}

		private void ApplyMaterialFixedPipeline()
		{
			if (this.material == null)
			{
				return;
			}
			this.ApplyMaterialTextures();
			GL.UseProgram(0);
			OpenTKHelper.Assert();
		}

		private ShaderTechniqueArgumentIndices ApplyMaterialShader(IVertexStreamSource mesh)
		{
			if (this.material == null)
			{
				return this.ApplyDefaultMaterial(mesh);
			}

			this.ApplyMaterialTextures();

			var shaderTechnique = this.material.ShaderTechnique.Resource as ShaderTechnique;
			if (shaderTechnique != null)
			{
				var programShader = this.GetShaderProgram(shaderTechnique);
				GL.UseProgram(programShader.ProgramId);
				OpenTKHelper.Assert();

				this.BindShaderArgs(ref programShader);
				return programShader;
			}

			var vso = new DefaultVertexShaderOptions
				{
					BITANGENT_STREAM = mesh.HasStream(Streams.Binormal, 0),
					COL_STREAM = mesh.HasStream(Streams.Color, 0),
					FAST_FOG = false,
					FOG = false,
					//!material.NoFog,
					LIGHT_AMBIENT = this.lighting && this.light.Enabled,
					LIGHT_DIFFUSE = this.lighting && this.light.Enabled && this.IsNotDisabledColor(this.light.Diffuse),
					LIGHT_SPECULAR = this.lighting && this.light.Enabled && this.IsNotDisabledColor(this.light.Specular),
					LIGHT_EMISSIVE = this.lighting && this.light.Enabled,
					NORM_STREAM = mesh.HasStream(Streams.Normal, 0),
					SKIN_MAJOR_BONE = false,
					SKINWEIGHT_STREAM = false,
					SKIN_NORMALS = false,
					TANGENT_STREAM = mesh.HasStream(Streams.Tangent, 0),
					UV0_STREAM = mesh.HasStream(Streams.TexCoord, 0),
					UV1_STREAM = mesh.HasStream(Streams.TexCoord, 1)
				};

			var fso = new DefaultFragmentShaderOptions
				{
					COL_STREAM = mesh.HasStream(Streams.Color, 0),
					FAST_FOG = false,
					FOG = false,
					//!material.NoFog,
					LIGHT_AMBIENT = this.lighting && this.light.Enabled,
					LIGHT_DIFFUSE = this.lighting && this.light.Enabled && this.IsNotDisabledColor(this.light.Diffuse),
					LIGHT_SPECULAR = this.lighting && this.light.Enabled && this.IsNotDisabledColor(this.light.Specular),
					LIGHT_EMISSIVE = this.lighting && this.light.Enabled,
					UV0_STREAM = mesh.HasStream(Streams.TexCoord, 0),
					UV1_STREAM = mesh.HasStream(Streams.TexCoord, 1),
					ALPHA_BLEND = (int)this.material.AlphaMode,
					ALPHA_TEST = (int)this.material.AlphaTestMode,
					BLEND = (int)this.material.BlendMode,
					EFFECT_PRESET = (int)this.material.EffectPreset,
					IW_GX_PLATFORM_TEGRA2 = false,
					TEX0 = !this.material.Texture0.IsEmpty,
					TEX1 = !this.material.Texture1.IsEmpty
				};

			ShaderTechniqueArgumentIndices p = this.shaders.GetProgram(new DefaultProgramOptions(vso, fso));
			GL.UseProgram(p.ProgramId);
			OpenTKHelper.Assert();

			this.BindShaderArgs(ref p);

			return p;
		}

		private void ApplyMaterialTextures()
		{
			this.SetTexture(0, this.material.Texture0.Resource as Texture);
			this.ApplyFiltering();
			this.SetTexture(1, this.material.Texture1.Resource as Texture);
			this.ApplyFiltering();
			this.SetTexture(2, this.material.Texture2.Resource as Texture);
			this.ApplyFiltering();
			this.SetTexture(3, this.material.Texture3.Resource as Texture);
			this.ApplyFiltering();
			GL.ActiveTexture(TextureUnit.Texture0);
		}

		private void BindShaderArgs(ref ShaderTechniqueArgumentIndices p)
		{
			this.EvalShaderTechniqueArguments();

			if (p.inPMVMat >= 0)
			{
				GL.UniformMatrix4(p.inPMVMat, false, ref this.args.inPMVMat);
			}
			if (p.inMVMat >= 0)
			{
				GL.UniformMatrix4(p.inMVMat, false, ref this.args.inMVMat);
			}
			if (p.inMVRotMat >= 0)
			{
				GL.UniformMatrix3(p.inMVRotMat, 9, false, BuildMat3x3(ref this.args.inMVRotMat));
			}
			if (p.inModelRotMat >= 0)
			{
				GL.UniformMatrix3(p.inModelRotMat, 9, false, BuildMat3x3(ref this.args.inModelRotMat));
			}
			if (p.inModelPos >= 0)
			{
				GL.Uniform3(p.inModelPos, ref this.args.inModelPos);
			}
			if (p.inCamPos >= 0)
			{
				GL.Uniform3(p.inCamPos, ref this.args.inCamPos);
			}
			if (p.inEyePos >= 0)
			{
				GL.Uniform3(p.inEyePos, ref this.args.inEyePos);
			}
			if (p.inAlphaTestValue >= 0)
			{
				GL.Uniform1(p.inAlphaTestValue, this.args.inAlphaTestValue);
			}
			if (p.inMaterialAmbient >= 0)
			{
				GL.Uniform4(p.inMaterialAmbient, this.args.inMaterialAmbient);
			}
			if (p.inMaterialDiffuse >= 0)
			{
				GL.Uniform4(p.inMaterialDiffuse, this.args.inMaterialDiffuse);
			}
			if (p.inSampler0 >= 0)
			{
				GL.Uniform1(p.inSampler0, 0);
			}
			if (p.inSampler1 >= 0)
			{
				GL.Uniform1(p.inSampler1, 1);
			}
			if (p.inSampler2 >= 0)
			{
				GL.Uniform1(p.inSampler2, 2);
			}
			if (p.inSampler3 >= 0)
			{
				GL.Uniform1(p.inSampler3, 3);
			}
			if (p.inEmissive >= 0)
			{
				GL.Uniform4(p.inEmissive, this.args.inEmissive);
			}
			if (p.inAmbient >= 0)
			{
				GL.Uniform4(p.inAmbient, this.args.inAmbient);
			}
			if (p.inDiffuse >= 0)
			{
				GL.Uniform4(p.inDiffuse, this.args.inDiffuse);
			}
			if (p.inDiffuseDir >= 0)
			{
				GL.Uniform3(p.inDiffuseDir, this.args.inDiffuseDir);
			}
			if (p.inFogNear >= 0)
			{
				GL.Uniform1(p.inFogNear, this.args.inFogNear);
			}
			if (p.inFogRange >= 0)
			{
				GL.Uniform1(p.inFogRange, this.args.inFogRange);
			}
			if (p.inFogColour >= 0)
			{
				GL.Uniform4(p.inFogColour, this.args.inFogColour);
			}
			if (p.inUVOffset >= 0)
			{
				GL.Uniform2(p.inUVOffset, this.args.inUVOffset);
			}

			if (p.inTVScale >= 0)
			{
				GL.Uniform2(p.inTVScale, this.args.inTVScale);
			}
			if (p.inUVScale >= 0)
			{
				GL.Uniform2(p.inUVScale, this.args.inUVScale);
			}
			if (p.inUV1Scale >= 0)
			{
				GL.Uniform2(p.inUV1Scale, this.args.inUV1Scale);
			}
			if (p.inUV2Scale >= 0)
			{
				GL.Uniform2(p.inUV2Scale, this.args.inUV2Scale);
			}
			if (p.inUV3Scale >= 0)
			{
				GL.Uniform2(p.inUV3Scale, this.args.inUV3Scale);
			}
			if (p.inTextureSize >= 0)
			{
				GL.Uniform2(p.inTextureSize, this.args.inTextureSize);
			}
			if (p.inTextureSize1 >= 0)
			{
				GL.Uniform2(p.inTextureSize1, this.args.inTextureSize1);
			}
			if (p.inTextureSize2 >= 0)
			{
				GL.Uniform2(p.inTextureSize2, this.args.inTextureSize2);
			}
			if (p.inTextureSize3 >= 0)
			{
				GL.Uniform2(p.inTextureSize3, this.args.inTextureSize3);
			}
			if (p.inOOTextureSize >= 0)
			{
				GL.Uniform2(p.inOOTextureSize, this.args.inOOTextureSize);
			}
			if (p.inOOTextureSize1 >= 0)
			{
				GL.Uniform2(p.inOOTextureSize1, this.args.inOOTextureSize1);
			}
			if (p.inOOTextureSize2 >= 0)
			{
				GL.Uniform2(p.inOOTextureSize2, this.args.inOOTextureSize2);
			}
			if (p.inOOTextureSize3 >= 0)
			{
				GL.Uniform2(p.inOOTextureSize3, this.args.inOOTextureSize3);
			}
			if (p.inDisplaySize >= 0)
			{
				GL.Uniform2(p.inDisplaySize, this.args.inDisplaySize);
			}
			if (p.inDeviceSize >= 0)
			{
				GL.Uniform2(p.inDeviceSize, this.args.inDeviceSize);
			}
			if (p.inDisplayRotScaleMat >= 0)
			{
				GL.UniformMatrix3(p.inDisplayRotScaleMat, 9, false, BuildMat3x3(ref this.args.inDisplayRotScaleMat));
			}
			if (p.inSpecular >= 0)
			{
				GL.Uniform4(p.inSpecular, this.args.inSpecular);
			}
			if (p.inMaterialSpecular >= 0)
			{
				GL.Uniform4(p.inMaterialSpecular, this.args.inMaterialSpecular);
			}
			if (p.inSpecularHalfVec >= 0)
			{
				GL.Uniform3(p.inSpecularHalfVec, this.args.inSpecularHalfVec);
			}

			//GL.UseProgram(0);
			OpenTKHelper.Assert();
		}

		private Material ConvertMaterial(IMaterial src)
		{
			var dst = new Material(this.resourceManager);
			dst.Name = src.Name;
			dst.DepthWriteEnable = true;
			if (src.Effect.Ambient != null)
			{
				dst.ColAmbient = src.Effect.Ambient.GetColor();
			}
			switch (src.Effect.CullMode)
			{
				case Utils.Mesh.CullMode.Front:
					dst.CullMode = CullMode.FRONT;
					break;
				case Utils.Mesh.CullMode.Back:
					dst.CullMode = CullMode.BACK;
					break;
				case Utils.Mesh.CullMode.None:
					dst.CullMode = CullMode.NONE;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			var diffuse = src.Effect.Diffuse;
			if (diffuse != null)
			{
				switch (diffuse.Type)
				{
					case ColorSourceType.SolidColor:
					case ColorSourceType.Function:
						dst.ColDiffuse = diffuse.GetColor();
						break;
					case ColorSourceType.Image:
						var emb = ((ImageColorSource)diffuse).Image as EmbeddedImage;
						if (emb != null)
						{
							dst.Texture0.Resource = new Texture
								{ Image = new Image((ushort)emb.Width, (ushort)emb.Height, emb.Pitch, ImageFormat.ABGR_8888, emb.GetRawData()) };
						}
						else
						{
							var fileReference = diffuse.GetImagePath();
							if (!string.IsNullOrEmpty(fileReference))
							{
								if (File.Exists(fileReference))
								{
									dst.Texture0.FileReference = fileReference;
								}
							}
						}
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			return dst;
		}

		private void DropVertextBuffer()
		{
			this.vertexBufferCount = 0;
			this.indexBufferCount = 0;
		}

		private void EvalShaderTechniqueArguments()
		{
			this.args.inPMVMat = this.model * this.view * this.projection;
			this.args.inMVMat = this.model * this.view;
			var invMMat = this.model;
			invMMat.Invert();
			this.args.inMVRotMat = this.args.inMVMat;
			this.args.inModelRotMat = this.model;
			this.args.inModelPos = Vector3.Transform(Vector3.Zero, this.model);
			Matrix4 invView = this.view;
			invView.Invert();
			this.args.inCamPos = Vector3.Transform(Vector3.Zero, invView);
			this.args.inDiffuseDir = Vector3.Transform(this.light.Position, invMMat);
			this.args.inDiffuseDir.Normalize();
			var cam = Vector3.Transform(this.args.inCamPos, invMMat);
			this.args.inEyePos = cam;
			cam.Normalize();
			cam = this.args.inDiffuseDir + cam;
			cam.Normalize();
			this.args.inSpecularHalfVec = cam;
			this.args.inEmissive = this.material == null ? Vector4.Zero : ColorToVector4(this.material.ColEmissive);
			this.args.inAmbient = (this.lighting && this.light.Enabled)
			                      	? ColorToVector4(this.light.Ambient)
			                      	: new Vector4(1, 1, 1, 1.0f);
			this.args.inDiffuse = (this.lighting && this.light.Enabled)
			                      	? ColorToVector4(this.light.Diffuse)
			                      	: new Vector4(1, 1, 1, 1.0f);
			this.args.inSpecular = (this.lighting && this.light.Enabled)
			                       	? ColorToVector4(this.light.Specular)
			                       	: new Vector4(1, 1, 1, 1.0f);
			this.args.inMaterialSpecular = this.material != null
			                               	? ColorToVector4(this.material.SpecularCombined)
			                               	: new Vector4(0, 0, 0, 0.0f);
			this.args.inMaterialAmbient = this.material == null ? Vector4.Zero : ColorToVector4(this.material.ColAmbient);
			this.args.inMaterialDiffuse = this.material == null ? Vector4.Zero : ColorToVector4(this.material.ColDiffuse);
		}

		private ShaderTechniqueArgumentIndices GetShaderProgram(ShaderTechnique shaderTechnique)
		{
			var shaderContext = shaderTechnique.ContextData as ShaderContext;
			if (shaderContext == null)
			{
				shaderTechnique.ContextData = shaderContext = new ShaderContext(shaderTechnique);
			}
			return shaderContext.Indices;
		}

		private bool IsNotDisabledColor(Color ambient)
		{
			if (ambient.A == 0)
			{
				return false;
			}
			if (ambient.R == 0 && ambient.G == 0 && ambient.B == 0)
			{
				return false;
			}
			return true;
		}

		private void Render(IVertexStreamSource vb, IVertexIndexSource indices)
		{
			BeginMode mode;
			int count = 0;
			switch (indices.VertexSourceType)
			{
				case VertexSourceType.TrianleList:
					mode = BeginMode.Triangles;
					break;
				case VertexSourceType.TrianleStrip:
					mode = BeginMode.TriangleStrip;
					break;
				case VertexSourceType.QuadList:
					mode = BeginMode.Quads;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			count = indices.Count;
			var ids = new uint[indices.Count];
			int i = 0;
			foreach (var index in indices)
			{
				ids[i] = (uint)index;
				++i;
			}
			GL.DrawElements(mode, count, DrawElementsType.UnsignedInt, ids);
			OpenTKHelper.Assert();
		}

		private void RenderMesh(Mesh mesh)
		{
			var vertexBufferRenderData = mesh.ContextData as VertexBufferRenderData;
			if (vertexBufferRenderData == null)
			{
				mesh.ContextData = vertexBufferRenderData = new VertexBufferRenderData(mesh);
			}

			foreach (var surface in mesh.Surfaces)
			{
				var mtl = surface.Material.Resource as Material;
				if (mtl == null && !string.IsNullOrEmpty(surface.Material.NameReference))
				{
					//TODO: fix naming
					mtl =
						this.resourceManager.FindResource(
							surface.Material.Type, Hash.Get(mesh.Name + "/" + surface.Material.NameReference)) as Material;
				}
				this.SetMaterial(mtl);

				var p = this.ApplyMaterialShader(mesh);
				vertexBufferRenderData.Enable(p);
				this.Render(mesh, surface);
				vertexBufferRenderData.Disable(p);
			}
		}

		private void RenderSurface(Mesh mesh, Surface surface)
		{
			this.Render(mesh, surface);
		}

		private void RenderVertexBuffer()
		{
			if (this.vertexBufferCount == 0)
			{
				return;
			}

			this.SetMaterial((Material)null);
			this.DisableLighting();

			//SetTexture(0, null);
			//SetTexture(1, null);
			//SetTexture(2, null);
			//SetTexture(3, null);

			this.SetModel(ref Matrix4.Identity);
			//this.SetView(ref Matrix4.Identity);

			//GL.Enable(EnableCap.CullFace);
			//GL.CullFace(CullFaceMode.Front);
			GL.Enable(EnableCap.DepthTest);

			GL.Begin(BeginMode.Lines);
			for (int i = 0; i < this.vertexBufferCount; ++i)
			{
				GL.Color4(this.vertexBuffer[i].Color);
				GL.Vertex3(this.vertexBuffer[i].Position);
			}
			GL.End();
			this.vertexBufferCount = 0;
			this.indexBufferCount = 0;
		}

		private void SetMaterial(IMaterial material)
		{
			if (material == null)
			{
				GL.UseProgram(0);
				return;
			}
			if (material.RenderData as Material == null)
			{
				material.RenderData = this.ConvertMaterial(material);
			}
			this.SetMaterial(material.RenderData as Material);
		}

		private void UpdateFrustum()
		{
			Frustum.BuildFrustum(ref this.view, ref this.projection, out this.frustum);
			if (this.vsdProvider != null)
			{
				this.vsdProvider.CameraPosition = this.frustum.CameraPosition;
			}
		}

		#endregion
	}
}
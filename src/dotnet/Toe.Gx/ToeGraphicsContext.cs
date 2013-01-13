using System;
using System.Collections.Generic;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Toe.Marmalade.IwGraphics;
using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwGx;
using Toe.Utils.Mesh;

namespace Toe.Gx
{
	public class ToeGraphicsContext : IDisposable
	{
		#region Constants and Fields

		private GraphicsContext context;

		private bool flipCulling = true;

		#endregion

		#region Constructors and Destructors

		private Material material;

		private DefaultShaders shaders = new DefaultShaders();

		public void SetMaterial(Material m)
		{
			this.material = m;

		}

		public ToeGraphicsContext()
		{
			GraphicsContext.ShareContexts = true;

			//context = OpenTK.Graphics.GraphicsContext.CreateDummyContext();
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

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region Methods

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.context != null)
				{
					this.context.Dispose();
					this.context = null;
				}
			}
		}

		#endregion

		private bool[] isTextureSet=new bool [4];

		public void SetTexture(int channel, Texture texture)
		{
			if (texture == null)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + channel);
				GL.Disable(EnableCap.Texture2D);
				isTextureSet[channel] = false;
				return;
			}
			isTextureSet[channel] = true;
			var textureContext = texture.ContextData as TextureContext;
			if (textureContext == null)
			{
				textureContext = new TextureContext(texture);
				texture.ContextData = textureContext;
			}
			textureContext.ApplyToChannel(channel);
		}

		public void RenderModel(Model model)
		{
			foreach (var mesh in model.Meshes)
			{
				RenderMesh(mesh);
			}
		}
		public void Render(IMesh mesh)
		{
			var vertexBufferRenderData = mesh.RenderData as VertexBufferRenderData;
			if (vertexBufferRenderData == null)
			{
				mesh.RenderData = vertexBufferRenderData = new VertexBufferRenderData(mesh);
			}
			
			foreach (var surface in mesh.Submeshes)
			{
				//this.SetMaterial(surface.Material.Resource as Material);

				var p = ApplyMaterialShader(mesh);
				if (p == null)
					return;
				vertexBufferRenderData.Enable(p);
				
				Render(mesh, surface);
				
			}
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
				this.SetMaterial(surface.Material.Resource as Material);

				var p = ApplyMaterialShader(mesh);	
				vertexBufferRenderData.Enable(p);
				Render(mesh, surface);
				vertexBufferRenderData.Disable(p);
			}
		}

		private void RenderSurface(Mesh mesh, Surface surface)
		{
			this.Render(mesh, surface);			
		}

		private void Render(IVertexStreamSource vb, IVertexIndexSource indices)
		{
			BeginMode mode;
			int count = 0;
			switch (indices.VertexSourceType)
			{
				case VertexSourceType.TrianleList:
					mode = BeginMode.Triangles;
					count = indices.Count / 3;
					break;
				case VertexSourceType.TrianleStrip:
					mode = BeginMode.TriangleStrip;
					count = indices.Count-2;
					break;
				case VertexSourceType.QuadList:
					mode = BeginMode.Quads;
					count = indices.Count /4;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			count = indices.Count;
			var ids = new uint[indices.Count];
			int i=0;
			foreach (var index in indices)
			{
				ids[i] = (uint)index;
				++i;
			}
			GL.DrawElements(mode, count,DrawElementsType.UnsignedInt, ids);
			OpenTKHelper.Assert();
		}

		private ShaderTechniqueArgumentIndices ApplyDefaultMaterial(IVertexStreamSource mesh)
		{
			var vso = new DefaultVertexShaderOptions
			{
				BITANGENT_STREAM = false,//mesh.IsBinormalStreamAvailable,
				COL_STREAM = mesh.IsColorStreamAvailable,
				FAST_FOG = false,
				FOG = false,//!material.NoFog,
				LIGHT_AMBIENT = lighting && light.Enabled,
				LIGHT_DIFFUSE = lighting && light.Enabled && IsNotDisabledColor(light.Diffuse),
				LIGHT_SPECULAR = lighting && light.Enabled && IsNotDisabledColor(light.Specular),
				LIGHT_EMISSIVE = lighting && light.Enabled,
				NORM_STREAM = mesh.IsNormalStreamAvailable,
				SKIN_MAJOR_BONE = false,
				SKINWEIGHT_STREAM = false,
				SKIN_NORMALS = false,
				TANGENT_STREAM = false,//mesh.IsTangentStreamAvailable,
				UV0_STREAM = mesh.IsUV0StreamAvailable && isTextureSet[0],
				UV1_STREAM = mesh.IsUV1StreamAvailable && isTextureSet[1]
			};

			var fso = new DefaultFragmentShaderOptions()
			{
				COL_STREAM = mesh.IsColorStreamAvailable,
				FAST_FOG = false,
				FOG = false,//!material.NoFog,
				LIGHT_AMBIENT = lighting && light.Enabled,
				LIGHT_DIFFUSE = lighting && light.Enabled && IsNotDisabledColor(light.Diffuse),
				LIGHT_SPECULAR = lighting && light.Enabled && IsNotDisabledColor(light.Specular),
				LIGHT_EMISSIVE = lighting && light.Enabled,
				UV0_STREAM = mesh.IsUV0StreamAvailable,
				UV1_STREAM = mesh.IsUV1StreamAvailable,
				ALPHA_BLEND = (int)AlphaMode.DEFAULT,
				ALPHA_TEST = (int)AlphaTestMode.DISABLED,
				BLEND = (int)BlendMode.MODULATE,
				EFFECT_PRESET = (int)EffectPreset.DEFAULT,
				IW_GX_PLATFORM_TEGRA2 = false,
				TEX0 = isTextureSet[0],
				TEX1 = isTextureSet[1]
			};

			ShaderTechniqueArgumentIndices p = shaders.GetProgram(new DefaultProgramOptions(vso, fso));
			GL.UseProgram(p.ProgramId);
			OpenTKHelper.Assert();

			this.BindShaderArgs(ref p);

			return p;
		}
		private ShaderTechniqueArgumentIndices ApplyMaterialShader(IVertexStreamSource mesh)
		{
			if (material == null)
			{
				return ApplyDefaultMaterial(mesh);
			}

			this.SetTexture(0, material.Texture0.Resource as Texture);
			this.ApplyFiltering();
			this.SetTexture(1, material.Texture1.Resource as Texture);
			this.ApplyFiltering();
			this.SetTexture(2, material.Texture1.Resource as Texture);
			this.ApplyFiltering();
			this.SetTexture(3, material.Texture1.Resource as Texture);
			this.ApplyFiltering();
			GL.ActiveTexture(TextureUnit.Texture0);

			var shaderTechnique = material.ShaderTechnique.Resource as ShaderTechnique;
			if (shaderTechnique!=null)
			{
				var programShader = GetShaderProgram(shaderTechnique);
				GL.UseProgram(programShader.ProgramId);
				OpenTKHelper.Assert();

				this.BindShaderArgs(ref programShader);
				return programShader;
			}

			var vso = new DefaultVertexShaderOptions
			{
				BITANGENT_STREAM = false,//mesh.IsBinormalStreamAvailable,
				COL_STREAM = mesh.IsColorStreamAvailable,
				FAST_FOG = false,
				FOG = false,//!material.NoFog,
				LIGHT_AMBIENT = lighting && light.Enabled,
				LIGHT_DIFFUSE = lighting && light.Enabled && IsNotDisabledColor(light.Diffuse),
				LIGHT_SPECULAR = lighting && light.Enabled && IsNotDisabledColor(light.Specular),
				LIGHT_EMISSIVE = lighting && light.Enabled,
				NORM_STREAM = mesh.IsNormalStreamAvailable,
				SKIN_MAJOR_BONE = false,
				SKINWEIGHT_STREAM = false,
				SKIN_NORMALS = false,
				TANGENT_STREAM = false,//mesh.IsTangentStreamAvailable,
				UV0_STREAM = mesh.IsUV0StreamAvailable,
				UV1_STREAM = mesh.IsUV1StreamAvailable
			};

			var fso = new DefaultFragmentShaderOptions()
			{
				COL_STREAM = mesh.IsColorStreamAvailable,
				FAST_FOG = false,
				FOG = false,//!material.NoFog,
				LIGHT_AMBIENT = lighting && light.Enabled,
				LIGHT_DIFFUSE = lighting && light.Enabled && IsNotDisabledColor(light.Diffuse),
				LIGHT_SPECULAR = lighting && light.Enabled && IsNotDisabledColor(light.Specular),
				LIGHT_EMISSIVE = lighting && light.Enabled,
				UV0_STREAM = mesh.IsUV0StreamAvailable,
				UV1_STREAM = mesh.IsUV1StreamAvailable,
				ALPHA_BLEND = (int)material.AlphaMode,
				ALPHA_TEST = (int)material.AlphaTestMode,
				BLEND = (int)material.BlendMode,
				EFFECT_PRESET = (int)material.EffectPreset,
				IW_GX_PLATFORM_TEGRA2 = false,
				TEX0 = !material.Texture0.IsEmpty,
				TEX1 = !material.Texture1.IsEmpty
			};

			ShaderTechniqueArgumentIndices p = shaders.GetProgram(new DefaultProgramOptions(vso, fso));
			GL.UseProgram(p.ProgramId);
			OpenTKHelper.Assert();

			this.BindShaderArgs(ref p);

			return p;
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
			if (ambient.A == 0) return false;
			if (ambient.R == 0 && ambient.G == 0 && ambient.B == 0) return false;
			return true;
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

		private static float[] BuildMat3x3(ref Matrix4 inModelRotMat)
		{
			return new float[] { inModelRotMat.M11, inModelRotMat.M12, inModelRotMat.M13, inModelRotMat.M21, inModelRotMat.M22, inModelRotMat.M23, inModelRotMat.M31, inModelRotMat.M32, inModelRotMat.M33 };
		}

		ShaderTechniqueArguments args = new ShaderTechniqueArguments();

		private void EvalShaderTechniqueArguments()
		{
			args.inPMVMat = model * view * projection;
			args.inMVMat = model*view;
			var invMMat = model;
			invMMat.Invert();
			args.inMVRotMat = args.inMVMat;
			args.inModelRotMat = model;
			args.inModelPos = Vector3.Transform(Vector3.Zero, model);
			Matrix4 invView = view;
			invView.Invert();
			args.inCamPos = Vector3.Transform(Vector3.Zero, invView);
			args.inDiffuseDir = Vector3.Transform(this.light.Position, invMMat);
			args.inDiffuseDir.Normalize();
			var cam = Vector3.Transform(args.inCamPos, invMMat);
			cam.Normalize();
			cam = args.inDiffuseDir + cam;
			cam.Normalize();
			args.inSpecularHalfVec = cam;
			args.inEmissive = material == null ? Vector4.Zero : ColorToVector4(material.ColEmissive);
			args.inAmbient = (lighting && light.Enabled)?ColorToVector4(light.Ambient):new Vector4(1, 1, 1, 1.0f);
			args.inDiffuse = (lighting && light.Enabled) ? ColorToVector4(light.Diffuse) : new Vector4(1, 1, 1, 1.0f);
			args.inSpecular = (lighting && light.Enabled) ? ColorToVector4(light.Specular) : new Vector4(1, 1, 1, 1.0f);
			args.inMaterialSpecular = material != null ? ColorToVector4(material.SpecularCombined) : new Vector4(0, 0, 0, 0.0f);
			args.inMaterialAmbient = material == null ? Vector4.Zero : ColorToVector4(material.ColAmbient);
			args.inMaterialDiffuse = material == null ? Vector4.Zero : ColorToVector4(material.ColDiffuse);
		}

		private static Vector4 ColorToVector4(Color colEmissive)
		{
			return new Vector4(colEmissive.R / 255.0f, colEmissive.G / 255.0f, colEmissive.B / 255.0f, colEmissive.A / 255.0f);
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

		//private void RenderVertex(IVertexSource mesh, Vertex vertex, ShaderTechniqueArgumentIndices indices)
		//{
		//    if (indices == null)
		//    {
		//        if (mesh.IsNormalStreamAvailable) GL.Normal3(vertex.Normal);
		//        if (mesh.IsColorStreamAvailable) GL.Color4(vertex.Color);
		//        if (mesh.IsUV0StreamAvailable) GL.MultiTexCoord3(TextureUnit.Texture0, vertex.UV0.X, vertex.UV0.Y, vertex.UV0.Z);
		//        if (mesh.IsUV1StreamAvailable) GL.MultiTexCoord3(TextureUnit.Texture1, vertex.UV1.X, vertex.UV1.Y, vertex.UV1.Z);
		//        GL.Vertex3(vertex.Position);
		//    }
		//    else
		//    {
		//        if (indices.inCol >= 0)
		//            if (mesh.IsColorStreamAvailable)
		//                GL.VertexAttrib4(indices.inCol, new Vector4( vertex.Color.R/255.0f, vertex.Color.G/255.0f, vertex.Color.B/255.0f, vertex.Color.A/255.0f ));
		//            else
		//                GL.VertexAttrib4(indices.inCol, new Vector4(1,1,1,1));
		//        if (indices.inNorm >= 0)
		//            if (mesh.IsNormalStreamAvailable)
		//                GL.VertexAttrib4(indices.inNorm, new Vector4(vertex.Normal));
		//            else
		//                GL.VertexAttrib4(indices.inNorm, new Vector4(0, 0, 1,1));
		//        if (indices.inUV0 >= 0)
		//            if (mesh.IsUV0StreamAvailable)
		//                GL.VertexAttrib2(indices.inUV0, new Vector2(vertex.UV0));
		//            else
		//                GL.VertexAttrib2(indices.inUV0, new Vector2(0, 0));
		//        if (indices.inUV1 >= 0)
		//            if (mesh.IsUV1StreamAvailable)
		//                GL.VertexAttrib2(indices.inUV1, new Vector2(vertex.UV1));
		//            else
		//                GL.VertexAttrib2(indices.inUV1, new Vector2(0, 0));
		//        if (indices.inBiTangent >= 0)
		//            //if (mesh.IsBinormalStreamAvailable)
		//            //    GL.VertexAttrib4(indices.inBiTangent, vertex.Binormal);
		//            //else
		//                GL.VertexAttrib4(indices.inBiTangent, new Vector4(0, 0, 0,1));
		//        if (indices.inTangent >= 0)
		//            //if (mesh.IsTangentStreamAvailable)
		//            //    GL.VertexAttrib4(indices.inTangent, vertex.Tangent);
		//            //else
		//                GL.VertexAttrib4(indices.inTangent, new Vector4(0, 0, 0,1));
		//        //if (indices.inVert >= 0)
		//        //    if (mesh.IsVertexStreamAvailable)
		//        //    {
		//        //        Vector3 position = vertex.Position;
		//        //        GL.VertexAttrib4(indices.inVert, new Vector4(position.X, position.Y, position.Z,1));
		//        //    }
		//        //    else
		//        //        GL.VertexAttrib3(indices.inVert, new Vector3(0, 0, 0));
		//        GL.Vertex3(vertex.Position);
		//    }
		//}
		public void EnableLighting()
		{
			GL.Enable(EnableCap.Lighting);
			lighting = true;
		}

		public void DisableLighting()
		{
			GL.Disable(EnableCap.Lighting);
			lighting = false;
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
				GL.Light(LightName.Light0, LightParameter.Position, new[] { light.Position.X, light.Position.Y, light.Position.Z, 1.0f });
			}
		}

		public void SetViewport(int x, int y, int w, int h)
		{
			GL.Viewport(x, y, w, h);
			args.inDisplaySize = new Vector2(w,h);
			args.inDeviceSize = new Vector2(w, h);

		}

		private Matrix4 projection = Matrix4.Identity;

		private Matrix4 view = Matrix4.Identity;

		private Matrix4 model = Matrix4.Identity;

		private Matrix4 modelView = Matrix4.Identity;

		private LightArgs light = new LightArgs();

		private bool lighting;

		public void SetProjection(ref Matrix4 projection)
		{
			this.projection = projection;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);
			GL.MatrixMode(MatrixMode.Modelview);
		}

		public void SetView(ref Matrix4 view)
		{
			this.view = view;
			this.modelView = view*model;
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelView);
		}

		public void SetModel(ref Matrix4 view)
		{
			this.model = model;
			this.modelView = view * model;
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelView);
		}
	}
}
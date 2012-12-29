using System;
using System.Collections.Generic;
using System.Diagnostics;

using OpenTK.Graphics.OpenGL;

using Toe.Gx;
using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{


	public class ShaderTechnique : Managed
	{
		private readonly IResourceManager resourceManager;

		public static readonly uint TypeHash = Hash.Get("CIwGxShaderTechnique");

		private int vertexShaderHandle;

		private int fragmentShaderHandle;

		private int shaderProgramHandle;

		private string vertexShaderSource;

		private string fragmentShaderSource;

		public ShaderTechnique(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		public string VertexShaderSource
		{
			get
			{
				return this.vertexShaderSource;
			}
			set
			{
				this.vertexShaderSource = value;
			}
		}

		public string FragmentShaderSource
		{
			get
			{
				return this.fragmentShaderSource;
			}
			set
			{
				this.fragmentShaderSource = value;
			}
		}

		#region Overrides of Managed

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion

		public void ApplyOpenGL()
		{
			var v = GL.GetString(StringName.Version);
			if (!string.IsNullOrEmpty(this.vertexShaderSource))
			{
				if (vertexShaderHandle == 0)
				{
					vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
					OpenTKHelper.Assert();
					GL.ShaderSource(vertexShaderHandle, AdaptSource( vertexShaderSource));
					OpenTKHelper.Assert();
					GL.CompileShader(vertexShaderHandle);
					OpenTKHelper.Assert();
					int compileStatus;
					GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out compileStatus);
					if (compileStatus == 0)
					{
						string shaderInfoLog;
						GL.GetShaderInfoLog(vertexShaderHandle, out shaderInfoLog);
						throw new ApplicationException(shaderInfoLog);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.fragmentShaderSource))
			{
				if (fragmentShaderHandle == 0)
				{
					fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
					OpenTKHelper.Assert();
					GL.ShaderSource(fragmentShaderHandle,  AdaptSource(  fragmentShaderSource));
					OpenTKHelper.Assert();
					GL.CompileShader(fragmentShaderHandle);
					OpenTKHelper.Assert();
					int compileStatus;
					GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out compileStatus);
					if (compileStatus == 0)
					{
						string shaderInfoLog;
						GL.GetShaderInfoLog(fragmentShaderHandle, out shaderInfoLog);
						throw new ApplicationException(shaderInfoLog);
					}
				}
			}
			if (shaderProgramHandle == 0)
			{
				shaderProgramHandle = GL.CreateProgram();
				OpenTKHelper.Assert();

				if (vertexShaderHandle != 0)
				{
					GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
					OpenTKHelper.Assert();
				}
				if (fragmentShaderHandle != 0)
				{
					GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
					OpenTKHelper.Assert();
				}

				GL.LinkProgram(shaderProgramHandle);

				string programInfoLog;
				GL.GetProgramInfoLog(shaderProgramHandle, out programInfoLog);
				Debug.WriteLine(programInfoLog);
				OpenTKHelper.Assert();
				
				foreach (var param in shaderParams)
				{
					param.Location = GL.GetUniformLocation(shaderProgramHandle, param.ParamName);
					OpenTKHelper.Assert();
				}
			}

			GL.UseProgram(shaderProgramHandle);
			OpenTKHelper.Assert();

			foreach (var param in shaderParams)
			{
				param.ApplyOpenGL(shaderProgramHandle);
			}

		}

		private string AdaptSource(string src)
		{
			src = src.Replace(" highp ", " ");
			src = src.Replace(" mediump ", " ");
			src = src.Replace(" lowp ", " ");
			src = src.Replace("\thighp ", " ");
			src = src.Replace("\tmediump ", " ");
			src = src.Replace("\tlowp ", " ");
			return src;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			DisposeOpenGLHandlers();
		}

		private void DisposeOpenGLHandlers()
		{
			if (shaderProgramHandle > 0)
			{
				GL.DeleteProgram(shaderProgramHandle);
				shaderProgramHandle = 0;
			}
			if (vertexShaderHandle > 0)
			{
				GL.DeleteShader(vertexShaderHandle);
				vertexShaderHandle = 0;
			}
			if (fragmentShaderHandle > 0)
			{
				GL.DeleteShader(fragmentShaderHandle);
				fragmentShaderHandle = 0;
			}
		}

		IList<ShaderTechniqueParam> shaderParams = new List<ShaderTechniqueParam>();
		public void AddParam(ShaderTechniqueParam shaderParam)
		{
			shaderParams.Add(shaderParam);
		}
	}
	
}
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.IwGx
{
	public class ShaderTechnique : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwGxShaderTechnique");

		private readonly IResourceManager resourceManager;

		private readonly IList<ShaderTechniqueParam> shaderParams = new List<ShaderTechniqueParam>();

		private int fragmentShaderHandle;

		private int shaderProgramHandle;

		private int vertexShaderHandle;

		#endregion

		#region Constructors and Destructors

		public ShaderTechnique(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public string FragmentShaderSource { get; set; }

		public string VertexShaderSource { get; set; }

		#endregion

		#region Public Methods and Operators

		public static string AdaptSource(string src)
		{
			src = src.Replace(" highp ", " ");
			src = src.Replace(" mediump ", " ");
			src = src.Replace(" lowp ", " ");

			// void performAlphaTest(lowp float val)
			src = src.Replace("(lowp ", "( ");

			src = src.Replace("\thighp ", " ");
			src = src.Replace("\tmediump ", " ");
			src = src.Replace("\tlowp ", " ");
			return src;
		}

		public void AddParam(ShaderTechniqueParam shaderParam)
		{
			this.shaderParams.Add(shaderParam);
		}

		#endregion

		//public void ApplyOpenGL()
		//{
		//    var v = GL.GetString(StringName.Version);
		//    if (!string.IsNullOrEmpty(this.vertexShaderSource))
		//    {
		//        if (this.vertexShaderHandle == 0)
		//        {
		//            this.vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
		//            OpenTKHelper.Assert();
		//            GL.ShaderSource(this.vertexShaderHandle, this.AdaptSource(this.vertexShaderSource));
		//            OpenTKHelper.Assert();
		//            GL.CompileShader(this.vertexShaderHandle);
		//            OpenTKHelper.Assert();
		//            int compileStatus;
		//            GL.GetShader(this.vertexShaderHandle, ShaderParameter.CompileStatus, out compileStatus);
		//            if (compileStatus == 0)
		//            {
		//                string shaderInfoLog;
		//                GL.GetShaderInfoLog(this.vertexShaderHandle, out shaderInfoLog);
		//                throw new ApplicationException(shaderInfoLog);
		//            }
		//        }
		//    }
		//    if (!string.IsNullOrEmpty(this.fragmentShaderSource))
		//    {
		//        if (this.fragmentShaderHandle == 0)
		//        {
		//            this.fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
		//            OpenTKHelper.Assert();
		//            GL.ShaderSource(this.fragmentShaderHandle, this.AdaptSource(this.fragmentShaderSource));
		//            OpenTKHelper.Assert();
		//            GL.CompileShader(this.fragmentShaderHandle);
		//            OpenTKHelper.Assert();
		//            int compileStatus;
		//            GL.GetShader(this.fragmentShaderHandle, ShaderParameter.CompileStatus, out compileStatus);
		//            if (compileStatus == 0)
		//            {
		//                string shaderInfoLog;
		//                GL.GetShaderInfoLog(this.fragmentShaderHandle, out shaderInfoLog);
		//                throw new ApplicationException(shaderInfoLog);
		//            }
		//        }
		//    }
		//    if (this.shaderProgramHandle == 0)
		//    {
		//        this.shaderProgramHandle = GL.CreateProgram();
		//        OpenTKHelper.Assert();

		//        if (this.vertexShaderHandle != 0)
		//        {
		//            GL.AttachShader(this.shaderProgramHandle, this.vertexShaderHandle);
		//            OpenTKHelper.Assert();
		//        }
		//        if (this.fragmentShaderHandle != 0)
		//        {
		//            GL.AttachShader(this.shaderProgramHandle, this.fragmentShaderHandle);
		//            OpenTKHelper.Assert();
		//        }

		//        GL.LinkProgram(this.shaderProgramHandle);

		//        string programInfoLog;
		//        GL.GetProgramInfoLog(this.shaderProgramHandle, out programInfoLog);
		//        Debug.WriteLine(programInfoLog);
		//        OpenTKHelper.Assert();

		//        foreach (var param in this.shaderParams)
		//        {
		//            param.Location = GL.GetUniformLocation(this.shaderProgramHandle, param.ParamName);
		//            OpenTKHelper.Assert();
		//        }
		//    }

		//    GL.UseProgram(this.shaderProgramHandle);
		//    OpenTKHelper.Assert();

		//    foreach (var param in this.shaderParams)
		//    {
		//        param.ApplyOpenGL(this.shaderProgramHandle);
		//    }
		//}

		#region Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.DisposeOpenGLHandlers();
		}

		private void DisposeOpenGLHandlers()
		{
			if (this.shaderProgramHandle > 0)
			{
				GL.DeleteProgram(this.shaderProgramHandle);
				this.shaderProgramHandle = 0;
			}
			if (this.vertexShaderHandle > 0)
			{
				GL.DeleteShader(this.vertexShaderHandle);
				this.vertexShaderHandle = 0;
			}
			if (this.fragmentShaderHandle > 0)
			{
				GL.DeleteShader(this.fragmentShaderHandle);
				this.fragmentShaderHandle = 0;
			}
		}

		#endregion
	}
}
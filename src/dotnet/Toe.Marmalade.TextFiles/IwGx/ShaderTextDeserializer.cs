using System.Globalization;

using OpenTK;

using Toe.Marmalade.IwGx;
using Toe.Resources;

namespace Toe.Marmalade.TextFiles.IwGx
{
	public class ShaderTextDeserializer : ITextDeserializer
	{
		#region Constants and Fields

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public ShaderTextDeserializer(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#endregion

		#region Public Properties

		public string DefaultFileExtension
		{
			get
			{
				return ".itx";
			}
		}

		public string Fragment { get; set; }

		public string Vertex { get; set; }

		#endregion

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			ShaderTechnique shader = new ShaderTechnique(this.resourceManager);
			shader.Name = defaultName;
			shader.BasePath = parser.BasePath;

			parser.Consume("CIwGxShaderTechnique");
			parser.Consume("{");
			for (;;)
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "name")
				{
					parser.Consume();
					shader.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "param")
				{
					this.ParseParam(parser, shader);
					continue;
				}
				if (attribute == "shader")
				{
					parser.Consume();
					attribute = parser.Lexem;
					if (attribute == "vertex")
					{
						this.ParseVertexShader(parser, shader);
					}
					else if (attribute == "fragment")
					{
						this.ParseFragmentShader(parser, shader);
					}
					else
					{
						parser.UnknownLexemError();
					}
					continue;
				}
				parser.UnknownLexemError();
			}
			return shader;
		}

		#endregion

		#region Methods

		private void ParseFloatParam(TextParser parser, ShaderTechnique shader, string paramName, int numArgs)
		{
			var a = new float[numArgs];
			for (int i = 0; i < a.Length; ++i)
			{
				a[i] = parser.ConsumeFloat();
			}
			shader.AddParam(new ShaderTechniqueFloatParam(paramName, a));
		}

		private void ParseFragmentShader(TextParser parser, ShaderTechnique shader)
		{
			parser.Consume();
			shader.FragmentShaderSource = parser.ConsumeBlock();
		}

		private void ParseParam(TextParser parser, ShaderTechnique shader)
		{
			parser.Consume("param");
			var paramName = parser.ConsumeString();
			var typeName = parser.ConsumeString();
			var numArgs = parser.ConsumeInt();
			switch (typeName)
			{
				case "float":
					this.ParseFloatParam(parser, shader, paramName, numArgs);
					break;
				case "vec3":
					this.ParseVec3Param(parser, shader, paramName, numArgs);
					break;
				default:
					parser.Error(string.Format(CultureInfo.InvariantCulture, "Unknown param type {0}", typeName));
					break;
			}
		}

		private void ParseVec3Param(TextParser parser, ShaderTechnique shader, string paramName, int numArgs)
		{
			var a = new Vector3[numArgs];
			for (int i = 0; i < a.Length; ++i)
			{
				var x = parser.ConsumeFloat();
				var y = parser.ConsumeFloat();
				var z = parser.ConsumeFloat();
				a[i] = new Vector3(x, y, z);
			}
			shader.AddParam(new ShaderTechniqueVec3Param(paramName, a));
		}

		private void ParseVertexShader(TextParser parser, ShaderTechnique shader)
		{
			parser.Consume();

			shader.VertexShaderSource = parser.ConsumeBlock();
		}

		#endregion
	}
}
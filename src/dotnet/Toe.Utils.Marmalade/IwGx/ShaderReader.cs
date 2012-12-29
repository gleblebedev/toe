using System.Globalization;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class ShaderReader
	{
		private readonly IResourceManager resourceManager;

		private string fragment;

		private string vertex;

		public ShaderReader(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		public Managed Parse(TextParser parser)
		{
			ShaderTechnique shader = new ShaderTechnique(this.resourceManager);
			shader.BasePath = parser.BasePath;

			parser.Consume("CIwGxShaderTechnique");
			parser.Consume("{");
			for (; ; )
			{
				var attribute = parser.GetLexem();
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
					attribute = parser.GetLexem();
					if (attribute == "vertex") this.ParseVertexShader(parser, shader);
					else if (attribute == "fragment") this.ParseFragmentShader(parser, shader);
					else parser.UnknownLexem();
					continue;
				}
				parser.UnknownLexem();
			}
			return shader;
		}

		private void ParseFragmentShader(TextParser parser, ShaderTechnique shader)
		{
			parser.Consume();
			shader.FragmentShaderSource = parser.ConsumeBlock();
		}

		private void ParseVertexShader(TextParser parser, ShaderTechnique shader)
		{
			parser.Consume();

			shader.VertexShaderSource = parser.ConsumeBlock();
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
				default:
					parser.Error(string.Format(CultureInfo.InvariantCulture, "Unknown param type {0}", typeName));
					break;
			}
		}

		private void ParseFloatParam(TextParser parser, ShaderTechnique shader, string paramName, int numArgs)
		{
			float[] a = new float[numArgs];
			for (int i = 0; i < numArgs; ++i) a[i] = parser.ConsumeFloat();
			shader.AddParam(new ShaderTechniqueFloatParam(paramName, a));
		}
	}
}
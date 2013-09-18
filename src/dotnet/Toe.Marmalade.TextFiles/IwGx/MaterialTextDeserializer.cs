using Autofac;

using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.TextFiles.IwGx
{
	public class MaterialTextDeserializer : ITextDeserializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IResourceManager resourceManager;

		#endregion

		#region Constructors and Destructors

		public MaterialTextDeserializer(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		#endregion

		#region Public Properties

		public string DefaultFileExtension
		{
			get
			{
				return ".mtl";
			}
		}

		#endregion

		//public IList<Material> Load(Stream stream)
		//{
		//    IList<Material> materials = new List<Material>();
		//    using (var source = new StreamReader(stream))
		//    {
		//        var parser = new TextParser(source, Directory.GetCurrentDirectory());
		//        materials.Add((Material)this.Parse(parser, TODO));
		//    }
		//    return materials;
		//}

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			Material material = this.context.Resolve<Material>();
			material.Name = defaultName;
			material.BasePath = parser.BasePath;
			parser.Consume("CIwMaterial");
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
					material.Name = parser.ConsumeString();
					continue;
				}

				if (attribute == "celW")
				{
					parser.Consume();
					this.EnsureAnim(material).CelW = parser.ConsumeByte();
					continue;
				}
				if (attribute == "celH")
				{
					parser.Consume();

					this.EnsureAnim(material).CelH = parser.ConsumeByte();
					continue;
				}
				if (attribute == "celNumU")
				{
					parser.Consume();
					this.EnsureAnim(material).CelNumU = parser.ConsumeByte();
					continue;
				}

				if (attribute == "clampUV")
				{
					parser.Consume();
					material.ClampUV = parser.ConsumeBool();
					continue;
				}
				if (attribute == "specularPower")
				{
					parser.Consume();
					material.SpecularPower = parser.ConsumeByte();
					continue;
				}
				if (attribute == "celNum")
				{
					parser.Consume();
					this.EnsureAnim(material).CelNum = parser.ConsumeByte();
					continue;
				}
				if (attribute == "celPeriod")
				{
					parser.Consume();
					this.EnsureAnim(material).CelPeriod = parser.ConsumeByte();
					continue;
				}
				if (attribute == "zDepthOfs")
				{
					parser.Consume();
					material.ZDepthOfs = parser.ConsumeShort();
					continue;
				}
				if (attribute == "modulateMode")
				{
					parser.Consume();
					material.ModulateMode = parser.ConsumeEnum<ModulateMode>();
					continue;
				}
				if (attribute == "alphaMode")
				{
					parser.Consume();
					material.AlphaMode = parser.ConsumeEnum<AlphaMode>();
					continue;
				}
				if (attribute == "cullMode")
				{
					parser.Consume();
					material.CullMode = parser.ConsumeEnum<CullMode>();
					continue;
				}

				if (attribute == "colAmbient")
				{
					parser.Consume();
					material.ColAmbient = parser.ConsumeColor();
					continue;
				}
				if (attribute == "colEmissive")
				{
					parser.Consume();
					material.ColEmissive = parser.ConsumeColor();
					continue;
				}
				if (attribute == "colDiffuse")
				{
					parser.Consume();
					material.ColDiffuse = parser.ConsumeColor();
					continue;
				}
				if (attribute == "colSpecular")
				{
					parser.Consume();
					material.ColSpecular = parser.ConsumeColor();
					continue;
				}
				if (attribute == "noFog")
				{
					parser.Consume();
					material.NoFog = parser.ConsumeBool();
					continue;
				}
				if (attribute == "texture0" || attribute == "mapDiffuse")
				{
					parser.Consume();
					parser.ConsumeResourceReference(material.Texture0, "textures");
					continue;
				}
				if (attribute == "texture1")
				{
					parser.Consume();
					parser.ConsumeResourceReference(material.Texture1, "textures");
					continue;
				}
				if (attribute == "texture2")
				{
					parser.Consume();
					parser.ConsumeResourceReference(material.Texture2, "textures");
					continue;
				}
				if (attribute == "texture3")
				{
					parser.Consume();
					parser.ConsumeResourceReference(material.Texture3, "textures");
					continue;
				}
				if (attribute == "effectPreset")
				{
					parser.Consume();
					material.EffectPreset = parser.ConsumeEnum<EffectPreset>();
					continue;
				}
				if (attribute == "shadeMode")
				{
					parser.Consume();
					material.ShadeMode = parser.ConsumeEnum<ShadeMode>();
					continue;
				}
				if (attribute == "blendMode")
				{
					parser.Consume();
					material.BlendMode = parser.ConsumeEnum<BlendMode>();
					continue;
				}
				if (attribute == "shaderTechnique")
				{
					parser.Consume();
					parser.ConsumeResourceReference(material.ShaderTechnique);
					continue;
				}
				if (attribute == "vertexShader")
				{
					parser.Consume();
					material.VertexShader = parser.ConsumeString();
					continue;
				}

				parser.UnknownLexemError();
			}
			return material;
		}

		#endregion

		#region Methods

		private MatAnim EnsureAnim(Material material)
		{
			if (material.MatAnim == null)
			{
				material.MatAnim = new MatAnim();
			}
			return material.MatAnim;
		}

		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Autofac;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class MtlReader
	{
		private readonly IResourceManager resourceManager;

		private readonly IComponentContext context;

		public MtlReader(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
		}

		public IList<Material> Load(Stream stream)
		{
			IList<Material> materials = new List<Material>();
			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source, Directory.GetCurrentDirectory());
				materials.Add((Material)Parse(parser));
			}
			return materials;
		}

		public Managed Parse(TextParser parser)
		{
			
			Material material = context.Resolve<Material>();
			material.BasePath = parser.BasePath;
			parser.Consume("CIwMaterial");
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
					material.Name = parser.ConsumeString();
					continue;
				}

				if (attribute == "celW")
				{
					parser.Consume();
					EnsureAnim(material).CelW = parser.ConsumeByte();
					continue;
				}
				if (attribute == "celH")
				{
					parser.Consume();

					EnsureAnim(material).CelH = parser.ConsumeByte();
					continue;
				}
				if (attribute == "celNumU")
				{
					parser.Consume();
					EnsureAnim(material).CelNumU = parser.ConsumeByte();
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
					material.SpecularPower = parser.ConsumeInt();
					continue;
				}
					if (attribute == "celNum")
				{
					parser.Consume();
					EnsureAnim(material).CelNum = parser.ConsumeByte();
					continue;
				}
					if (attribute == "celPeriod")
				{
					parser.Consume();
					EnsureAnim(material).CelPeriod = parser.ConsumeByte();
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
						material.AlphaMode = parser.ConsumeEnum <AlphaMode>();
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
				
				if (attribute == "texture0" || attribute == "mapDiffuse")
					{
						parser.Consume();
						parser.ConsumeResourceReference(material.Texture0);
						continue;
					}
					if (attribute == "texture1")
					{
						parser.Consume();
						parser.ConsumeResourceReference(material.Texture1);
						continue;
					}
					if (attribute == "texture2")
					{
						parser.Consume();
						parser.ConsumeResourceReference(material.Texture2);
						continue;
					}
					if (attribute == "texture3")
					{
						parser.Consume();
						parser.ConsumeResourceReference(material.Texture3);
						continue;
					}
					if (attribute == "effectPreset")
					{
						parser.Consume();
						material.EffectPreset = parser.ConsumeEnum<EffectPreset>();
						continue;
					}
					if (attribute == "blendMode")
					{
						parser.Consume();
						switch (parser.ConsumeString())
						{
							case "ADD":
								material.BlendMode = BlendMode.ADD;
								break;
							case "BLEND":
								material.BlendMode = BlendMode.BLEND;
								break;
							case "DECAL":
								material.BlendMode = BlendMode.DECAL;
								break;
							case "MODULATE":
								material.BlendMode = BlendMode.MODULATE;
								break;
							case "MODULATE_2X":
								material.BlendMode = BlendMode.MODULATE_2X;
								break;
							case "MODULATE_4X":
								material.BlendMode = BlendMode.MODULATE_4X;
								break;
							case "REPLACE":
								material.BlendMode = BlendMode.REPLACE;
								break;
							default:
								throw new TextParserException("Unknown blendMode");
						}
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

					parser.UnknownLexem();
			}
			return material;
		}

		private MatAnim EnsureAnim(Material material)
		{
			if (material.MatAnim == null)
				material.MatAnim = new MatAnim();
			return material.MatAnim;
		}
	}
}

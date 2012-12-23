using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	public class MtlReader
	{
		public IList<Material> Load(Stream stream)
		{
			IList<Material> materials = new List<Material>();
			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source);
				materials.Add((Material)Parse(parser));
			}
			return materials;
		}

		public Managed Parse(TextParser parser)
		{
			Material material = new Material();
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
					material.CelW = parser.ConsumeFloat();
					continue;
				}
				if (attribute == "celH")
				{
					parser.Consume();
					material.CelH = parser.ConsumeFloat();
					continue;
				}
				if (attribute == "celNumU")
				{
					parser.Consume();
					material.celNumU = parser.ConsumeFloat();
					continue;
				}

				if (attribute == "clampUV")
				{
					parser.Consume();
					material.clampUV = parser.ConsumeBool();
					continue;
				}
				if (attribute == "specularPower")
				{
					parser.Consume();
					material.specularPower = parser.ConsumeInt();
					continue;
				}
					if (attribute == "celNum")
				{
					parser.Consume();
					material.celNum = parser.ConsumeFloat();
					continue;
				}
					if (attribute == "celPeriod")
				{
					parser.Consume();
					material.celPeriod = parser.ConsumeFloat();
					continue;
				}
				
					if (attribute == "alphaMode")
					{
						parser.Consume();
						material.alphaMode = parser.ConsumeString();
						continue;
					}
					if (attribute == "cullMode")
					{
						parser.Consume();
						material.cullMode = parser.ConsumeString();
						continue;
					}
			
					if (attribute == "colAmbient")
					{
						parser.Consume();
						material.colAmbient = parser.ConsumeColor();
						continue;
					}
				if (attribute == "colEmissive")
					{
						parser.Consume();
						material.colEmissive = parser.ConsumeColor();
						continue;
					}
				if (attribute == "colDiffuse")
					{
						parser.Consume();
						material.colDiffuse = parser.ConsumeColor();
						continue;
					}
				if (attribute == "colSpecular")
					{
						parser.Consume();
						material.colSpecular = parser.ConsumeColor();
						continue;
					}
				
				if (attribute == "texture0" || attribute == "mapDiffuse")
					{
						parser.Consume();
						material.Texture0 = parser.ConsumeString();
						continue;
					}
					if (attribute == "texture1")
					{
						parser.Consume();
						material.Texture1 = parser.ConsumeString();
						continue;
					}
					if (attribute == "texture2")
					{
						parser.Consume();
						material.texture2 = parser.ConsumeString();
						continue;
					}
					if (attribute == "effectPreset")
					{
						parser.Consume();
						material.effectPreset = parser.ConsumeString();
						continue;
					}
					if (attribute == "blendMode")
					{
						parser.Consume();
						material.blendMode = parser.ConsumeString();
						continue;
					}
					if (attribute == "shaderTechnique")
					{
						parser.Consume();
						material.shaderTechnique = parser.ConsumeString();
						continue;
					}
					if (attribute == "vertexShader")
					{
						parser.Consume();
						material.vertexShader = parser.ConsumeString();
						continue;
					}

					parser.UnknownLexem();
			}
			return material;
		}
	}
}

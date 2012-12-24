﻿using System;
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
				var parser = new TextParser(source, Directory.GetCurrentDirectory());
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
						material.Texture2 = parser.ConsumeString();
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
								material.BlendMode = BlendMode.BLEND_ADD;
								break;
							case "BLEND":
								material.BlendMode = BlendMode.BLEND_BLEND;
								break;
							case "DECAL":
								material.BlendMode = BlendMode.BLEND_DECAL;
								break;
							case "MODULATE":
								material.BlendMode = BlendMode.BLEND_MODULATE;
								break;
							case "BLEND_MODULATE_2X":
								material.BlendMode = BlendMode.BLEND_MODULATE_2X;
								break;
							case "BLEND_MODULATE_4X":
								material.BlendMode = BlendMode.BLEND_MODULATE_4X;
								break;
							case "BLEND_REPLACE":
								material.BlendMode = BlendMode.BLEND_REPLACE;
								break;
							default:
								throw new TextParserException("Unknown blendMode");
						}
						continue;
					}
					if (attribute == "shaderTechnique")
					{
						parser.Consume();
						material.ShaderTechnique = parser.ConsumeString();
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

		private MatAnim EnsureAnim(Material material)
		{
			if (material.MatAnim == null)
				material.MatAnim = new MatAnim();
			return material.MatAnim;
		}
	}
}

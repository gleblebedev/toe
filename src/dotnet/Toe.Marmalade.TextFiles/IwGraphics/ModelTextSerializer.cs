using System;
using System.Linq;

using Toe.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.TextFiles.IwGraphics
{
	public class ModelTextSerializer : ResourceTextSerializer, ITextSerializer
	{
		#region Public Properties

		/// <summary>
		/// Default file extension for text resource file for this particular resource.
		/// </summary>
		public override string DefaultFileExtension
		{
			get
			{
				return ".geo";
			}
		}

		#endregion

		#region Properties

		protected override string ClassName
		{
			get
			{
				return "CIwModel";
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse text block.
		/// </summary>
		public override void SerializeContent(TextSerializer parser, Managed managed)
		{
			base.SerializeContent(parser, managed);
			var variables = (Model)managed;
			int i = 1;
			foreach (var mesh in variables.Meshes)
			{
				parser.WriteAttribute("CMesh");
				parser.OpenBlock();
				parser.WriteAttribute("name");
				parser.WriteStringValue(mesh.Name,"mesh"+i);

				parser.WriteAttribute("scale");
				parser.WriteFloatValue(1);

				if (mesh.IsVertexStreamAvailable)
				{
					parser.WriteAttribute("CVerts");
					parser.OpenBlock();
					parser.WriteAttribute("numVerts");
					parser.WriteIntValue(mesh.Vertices.Count);
					foreach (var vector3 in mesh.Vertices)
					{
						parser.WriteAttribute("v");
						parser.WriteRaw(" {");
						parser.WriteFloatValue(vector3.X);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.Y);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.Z);
						parser.WriteRaw(" }");
					}
					parser.CloseBlock();
				}

				if (mesh.IsNormalStreamAvailable)
				{
					parser.WriteAttribute("CVertNorms");
					parser.OpenBlock();
					parser.WriteAttribute("numVertNorms");
					parser.WriteIntValue(mesh.Normals.Count);
					foreach (var vector3 in mesh.Normals)
					{
						parser.WriteAttribute("vn");
						parser.WriteRaw(" {");
						parser.WriteFloatValue(vector3.X);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.Y);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.Z);
						parser.WriteRaw(" }");
					}
					parser.CloseBlock();
				}

				if (mesh.IsColorStreamAvailable)
				{
					parser.WriteAttribute("CVertCols");
					parser.OpenBlock();
					parser.WriteAttribute("numVertCols");
					parser.WriteIntValue(mesh.Colors.Count);
					foreach (var vector3 in mesh.Colors)
					{
						parser.WriteAttribute("col");
						parser.WriteRaw(" {");
						parser.WriteFloatValue(vector3.R / 255.0f);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.G / 255.0f);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.B / 255.0f);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.A/255.0f);
						parser.WriteRaw(" }");
					}
					parser.CloseBlock();
				}

				if (mesh.IsUV0StreamAvailable)
				{
					parser.WriteAttribute("CUVs");
					parser.OpenBlock();
					parser.WriteAttribute("setID");
					parser.WriteIntValue(0);
					parser.WriteAttribute("numUVs");
					parser.WriteIntValue(mesh.UV0.Count);
					foreach (var vector3 in mesh.UV0)
					{
						parser.WriteAttribute("uv");
						parser.WriteRaw(" {");
						parser.WriteFloatValue(vector3.X);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.Y);
						parser.WriteRaw(" }");
					}
					parser.CloseBlock();
				}

				if (mesh.IsUV1StreamAvailable)
				{
					parser.WriteAttribute("CUVs");
					parser.OpenBlock();
					parser.WriteAttribute("setID");
					parser.WriteIntValue(1);
					parser.WriteAttribute("numUVs");
					parser.WriteIntValue(mesh.UV1.Count);
					foreach (var vector3 in mesh.UV1)
					{
						parser.WriteAttribute("uv");
						parser.WriteRaw(" {");
						parser.WriteFloatValue(vector3.X);
						parser.WriteRaw(",");
						parser.WriteFloatValue(vector3.Y);
						parser.WriteRaw(" }");
					}
					parser.CloseBlock();
				}
				foreach (var surface in mesh.Surfaces)
				{
					parser.WriteAttribute("CSurface");
					parser.OpenBlock();
					parser.WriteAttribute("material");
					parser.WriteStringValue(surface.Material.NameReference,"noskin");

					switch (surface.VertexSourceType)
					{
						case VertexSourceType.TriangleList:
							WriteTriList(parser, surface, mesh);
							break;
						//case VertexSourceType.TrianleStrip:
						//	break;
						//case VertexSourceType.QuadList:
						//	break;
						//case VertexSourceType.QuadStrip:
						//	break;
						//case VertexSourceType.LineLine:
						//	break;
						//case VertexSourceType.LineStrip:
						//	break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					WriteTriList(parser, surface, mesh);
					parser.CloseBlock();
				}

				parser.CloseBlock();
				++i;
			}
		}

		private static void WriteTriList(TextSerializer parser, Surface surface, Mesh mesh)
		{
			parser.WriteAttribute("CTris");
			parser.OpenBlock();
			parser.WriteAttribute("numTris");
			parser.WriteIntValue(surface.Count / 3);
			int tri = 0;

			var posIndex = surface.GetIndexReader(Streams.Position, 0);
			var posEnumerator = (posIndex != null)?posIndex.GetEnumerator():null;

			var normalIndex = surface.GetIndexReader(Streams.Normal, 0);
			var normalEnumerator = (normalIndex != null) ? normalIndex.GetEnumerator() : null;

			var uv0Index = surface.GetIndexReader(Streams.TexCoord, 0);
			var uv0Enumerator = (uv0Index != null) ? uv0Index.GetEnumerator() : null;

			var uv1Index = surface.GetIndexReader(Streams.TexCoord, 1);
			var uv1Enumerator = (uv1Index != null) ? uv1Index.GetEnumerator() : null;


			var colorIndex = surface.GetIndexReader(Streams.Color, 0);
			var colorEnumerator = (colorIndex != null) ? colorIndex.GetEnumerator() : null;

			for (int index = 0; index < surface.Count; ++index)
			{
				if (tri % 3 == 0)
				{
					parser.WriteAttribute("t");
				}
				++tri;
				parser.WriteRaw(" {");

				if (posEnumerator != null)
				{
					posEnumerator.MoveNext();
					parser.WriteIntValue(posEnumerator.Current);
				}
				else
				{
					parser.WriteIntValue(-1);
				}

				parser.WriteRaw(",");

				if (normalEnumerator != null)
				{
					normalEnumerator.MoveNext();
					parser.WriteIntValue(normalEnumerator.Current);
				}
				else
				{
					parser.WriteIntValue(-1);
				}


				parser.WriteRaw(",");

				if (uv0Enumerator != null)
				{
					uv0Enumerator.MoveNext();
					parser.WriteIntValue(uv0Enumerator.Current);
				}
				else
				{
					parser.WriteIntValue(-1);
				}

				parser.WriteRaw(",");

				if (uv1Enumerator != null)
				{
					uv1Enumerator.MoveNext();
					parser.WriteIntValue(uv1Enumerator.Current);
				}
				else
				{
					parser.WriteIntValue(-1);
				}

				parser.WriteRaw(",");

				if (colorEnumerator != null)
				{
					colorEnumerator.MoveNext();
					parser.WriteIntValue(colorEnumerator.Current);
				}
				else
				{
					parser.WriteIntValue(-1);
				}

				parser.WriteRaw(" }");
			}
			parser.CloseBlock();
		}

		#endregion
	}
}
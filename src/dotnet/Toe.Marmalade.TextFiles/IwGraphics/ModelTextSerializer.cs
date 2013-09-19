using Toe.Marmalade.IwGraphics;

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

				foreach (var surface in mesh.Surfaces)
				{
					parser.WriteAttribute("CSurface");
					parser.OpenBlock();
					parser.WriteAttribute("material");
					parser.WriteStringValue(surface.Material.NameReference,"noskin");
					parser.WriteAttribute("CTris");
					parser.OpenBlock();
					parser.WriteAttribute("numTris");
					parser.WriteIntValue(surface.Count);
					//foreach (var index in surface)
					//{
					//	parser.WriteAttribute("t");
					//	parser.WriteRaw(" {");
					//	parser.WriteRaw(",");
					//	parser.WriteFloatValue(vector3.Y);
					//	parser.WriteRaw(",");
					//	parser.WriteFloatValue(vector3.Z);
					//	parser.WriteRaw(" }");
					//}
					parser.CloseBlock();
					parser.CloseBlock();
				}

				parser.CloseBlock();
				++i;
			}
		}

		#endregion
	}
}
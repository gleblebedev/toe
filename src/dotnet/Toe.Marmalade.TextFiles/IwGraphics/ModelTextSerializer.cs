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
			foreach (var mesh in variables.Meshes)
			{
				parser.WriteAttribute("CMesh");
				parser.OpenBlock();
				parser.WriteAttribute("name");
				parser.WriteStringValue(mesh.Name);

				parser.WriteAttribute("scale");
				parser.WriteFloatValue(1);

				parser.WriteAttribute("CVerts");
				parser.OpenBlock();
				parser.WriteAttribute("numVerts");
				parser.WriteIntValue(0);
				parser.CloseBlock();

				foreach (var surface in mesh.Surfaces)
				{
					parser.WriteAttribute("CSurface");
					parser.OpenBlock();
					parser.WriteAttribute("material");
					parser.WriteStringValue(surface.Material.NameReference,"noskin");
					parser.WriteAttribute("CTris");
					parser.OpenBlock();
					parser.WriteAttribute("numTris");
					parser.WriteIntValue(0);
					parser.CloseBlock();
					parser.CloseBlock();
				}

				parser.CloseBlock();
			}
		}

		#endregion
	}
}
using System.Globalization;
using System.IO;

namespace Toe.Utils.Mesh.Marmalade
{
	public class SkelReader : IMeshReader
	{
		#region Public Methods and Operators

		public IMesh Load(Stream stream)
		{
			var mesh = new StreamMesh();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source);

				parser.Consume("CIwAnimSkel");
				parser.Consume("{");

				for (;;)
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
						mesh.Name = parser.ConsumeString();
						continue;
					}
					if (attribute == "numBones")
					{
						parser.Consume();
						mesh.Bones.Capacity = parser.ConsumeInt();
						continue;
					}

					if (attribute == "CIwAnimBone")
					{
						parser.Consume();
						ParseBone(parser, mesh);
						continue;
					}
					throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
				}
			}

			return mesh;
		}

		#endregion

		#region Methods

		private static void ParseBone(TextParser parser, StreamMesh mesh)
		{
			parser.Consume("{");
			MeshBone bone = null;
			for (;;)
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
					bone = mesh.Bones[mesh.EnsureBone(parser.ConsumeString())];
					continue;
				}
				if (attribute == "parent")
				{
					parser.Consume();
					bone.Parent = mesh.EnsureBone(parser.ConsumeString());
					continue;
				}
				if (attribute == "pos")
				{
					parser.Consume();
					bone.Pos = parser.ConsumeVector3();
					continue;
				}
				if (attribute == "rot")
				{
					parser.Consume();
					bone.Rot = parser.ConsumeQuaternion();
					continue;
				}
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		#endregion
	}
}
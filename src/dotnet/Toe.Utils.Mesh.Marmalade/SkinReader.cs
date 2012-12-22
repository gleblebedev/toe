using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Toe.Utils.Mesh.Marmalade
{
	public class SkinReader : IMeshReader
	{
		#region Public Methods and Operators

		public IMesh Load(Stream stream)
		{
			var mesh = new StreamMesh();

			using (var source = new StreamReader(stream))
			{
				var parser = new TextParser(source);

				parser.Consume("CIwAnimSkin");
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
					if (attribute == "skeleton")
					{
						parser.Consume();
						mesh.Skeleton = parser.ConsumeString();
						continue;
					}
					if (attribute == "model")
					{
						parser.Consume();
						mesh.SkeletonModel = parser.ConsumeString();
						continue;
					}
					if (attribute == "CIwAnimSkinSet")
					{
						parser.Consume();
						ParseAnimSkinSet(parser, mesh);
						continue;
					}

					throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
				}
			}

			return mesh;
		}

		#endregion

		#region Methods

		private static void ParseAnimSkinSet(TextParser parser, StreamMesh mesh)
		{
			List<int> bones = new List<int>(4);
			parser.Consume("{");
			for (;;)
			{
				var attribute = parser.GetLexem();
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "useBones")
				{
					parser.Consume();
					parser.Consume("{");
					for (;;)
					{
						attribute = parser.GetLexem();
						if (attribute == "}")
						{
							parser.Consume();
							break;
						}
						bones.Add(mesh.EnsureBone(parser.ConsumeString()));
						parser.Skip(",");
					}
					continue;
				}
				if (attribute == "numVerts")
				{
					parser.Consume();
					parser.ConsumeInt();
					continue;
				}
				if (attribute == "vertWeights")
				{
					parser.Consume();
					ParseVertWeights(parser, bones, mesh);
					continue;
				}
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Unknown attribute {0}", attribute));
			}
		}

		private static void ParseVertWeights(TextParser parser, List<int> bones, StreamMesh mesh)
		{
			parser.Consume("{");
			var bone = parser.ConsumeInt();
			mesh.Weights.EnsureAt(bone);
			parser.Skip(",");
			VertexWeights w = VertexWeights.Empty;
			if (bones.Count > 0)
			{
				w.Bone0 = new VertexWeight { BoneIndex = bones[0], Weight = parser.ConsumeFloat() };
				parser.Skip(",");
				if (bones.Count > 1)
				{
					w.Bone1 = new VertexWeight { BoneIndex = bones[1], Weight = parser.ConsumeFloat() };
					parser.Skip(",");
					if (bones.Count > 2)
					{
						w.Bone2 = new VertexWeight { BoneIndex = bones[2], Weight = parser.ConsumeFloat() };
						parser.Skip(",");
						if (bones.Count > 3)
						{
							w.Bone3 = new VertexWeight { BoneIndex = bones[3], Weight = parser.ConsumeFloat() };
							parser.Skip(",");
							if (bones.Count > 4)
							{
								throw new TextParserException("max 4 bones supported");
							}
						}
					}
				}
			}
			parser.Consume("}");
		}

		#endregion
	}
}
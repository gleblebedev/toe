using System.Collections.Generic;

using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Utils.Mesh;
using Toe.Utils.TextParser;

namespace Toe.Marmalade.TextFiles.IwAnim
{
	public class SkinTextDeserializer : ITextDeserializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public SkinTextDeserializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Properties

		public string DefaultFileExtension
		{
			get
			{
				return ".skin";
			}
		}

		#endregion

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			AnimSkin skin = this.context.Resolve<AnimSkin>();
			skin.Name = defaultName;
			parser.Consume("CIwAnimSkin");
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
					skin.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "skeleton")
				{
					parser.Consume();
					parser.ConsumeResourceReference(skin.Skeleton);
					continue;
				}
				if (attribute == "model")
				{
					parser.Consume();
					parser.ConsumeResourceReference(skin.SkeletonModel);
					continue;
				}
				if (attribute == "CIwAnimSkinSet")
				{
					parser.Consume();
					ParseAnimSkinSet(parser, skin);
					continue;
				}

				parser.UnknownLexemError();
			}
			return skin;
		}

		#endregion

		#region Methods

		private static void ParseAnimSkinSet(TextParser parser, AnimSkin mesh)
		{
			List<int> bones = new List<int>(4);
			parser.Consume("{");
			for (;;)
			{
				var attribute = parser.Lexem;
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
						attribute = parser.Lexem;
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
				parser.UnknownLexemError();
			}
		}

		private static void ParseVertWeights(TextParser parser, List<int> bones, AnimSkin mesh)
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
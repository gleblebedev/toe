using Toe.Marmalade.IwAnim;
using Toe.Utils.Marmalade;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.TextFiles.IwAnim
{
	public class SkelTextSerializer : ITextSerializer
	{
		#region Public Properties

		public string DefaultFileExtension
		{
			get
			{
				return ".skel";
			}
		}

		#endregion

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			var skel = new AnimSkel();
			skel.Name = defaultName;
			parser.Consume("CIwAnimSkel");
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
					skel.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "numBones")
				{
					parser.Consume();
					skel.Bones.Capacity = parser.ConsumeInt();
					continue;
				}

				if (attribute == "CIwAnimBone")
				{
					parser.Consume();
					ParseBone(parser, skel);
					continue;
				}
				parser.UnknownLexemError();
			}
			return skel;
		}

		#endregion

		#region Methods

		private static void ParseBone(TextParser parser, AnimSkel mesh)
		{
			parser.Consume("{");
			MeshBone bone = null;
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
					bone.BindingPos = parser.ConsumeVector3();
					continue;
				}
				if (attribute == "rot")
				{
					parser.Consume();
					bone.BindingRot = parser.ConsumeQuaternion();
					continue;
				}
				parser.UnknownLexemError();
			}
		}

		#endregion
	}
}
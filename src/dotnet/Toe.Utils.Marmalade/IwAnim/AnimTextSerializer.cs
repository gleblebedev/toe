using Autofac;

using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Utils.Marmalade.IwAnim
{
	public class AnimTextSerializer : ITextSerializer
	{
		private readonly IComponentContext context;

		public AnimTextSerializer(IComponentContext context)
		{
			this.context = context;
		}

		#region Implementation of ITextSerializer

		/// <summary>
		/// Default file extension for text resource file for this particular resource.
		/// </summary>
		public string DefaultFileExtension
		{
			get
			{
				return ".anim";
			}
		}

		public Managed Parse(TextParser parser, string defaultName)
		{
			Anim mesh = context.Resolve<Anim>();
			mesh.Name = defaultName;
			parser.Consume("CIwAnim");
			parser.Consume("{");

			for (; ; )
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
					mesh.Name = parser.ConsumeString();
					continue;
				}
				if (attribute == "skeleton")
				{
					parser.Consume();
					parser.ConsumeResourceReference(mesh.Skeleton);
					continue;
				}
				if (attribute == "CIwAnimKeyFrame")
				{
					this.ParseKeyFrame(parser, mesh);
					continue;
				}
				parser.UnknownLexem();
			}
			return mesh;
		}

		private void ParseKeyFrame(TextParser parser, Anim mesh)
		{
			var frame = new AnimKeyFrame();
			parser.Consume("CIwAnimKeyFrame");
			parser.Consume("{");

			MeshBone bone = null;
			for (; ; )
			{
				var attribute = parser.Lexem;
				if (attribute == "}")
				{
					parser.Consume();
					break;
				}
				if (attribute == "time")
				{
					parser.Consume();
					frame.Time = parser.ConsumeFloat();
					continue;
				}
				if (attribute == "bone")
				{
					parser.Consume();
					bone = frame.Bones[frame.Bones.EnsureBone(parser.ConsumeString())];
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
				parser.UnknownLexem();
			}
			mesh.AddFrame(frame);
		}

		#endregion
	}
}
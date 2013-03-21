using Autofac;

using Toe.Marmalade.IwAnim;

namespace Toe.Marmalade.TextFiles.IwAnim
{
	public class AnimTextSerializer : ITextSerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public AnimTextSerializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Properties

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

		#endregion

		#region Public Methods and Operators

		public Managed Parse(TextParser parser, string defaultName)
		{
			Anim mesh = this.context.Resolve<Anim>();
			mesh.Name = defaultName;
			parser.Consume("CIwAnim");
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
				parser.UnknownLexemError();
			}
			return mesh;
		}

		#endregion

		#region Methods

		private void ParseKeyFrame(TextParser parser, Anim mesh)
		{
			var frame = this.context.Resolve<AnimKeyFrame>();
			parser.Consume("CIwAnimKeyFrame");
			parser.Consume("{");

			AnimBone bone = null;
			for (;;)
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
					bone = frame.Bones[frame.Bones.EnsureItem(parser.ConsumeString())];
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
			mesh.AddFrame(frame);
		}

		#endregion
	}
}
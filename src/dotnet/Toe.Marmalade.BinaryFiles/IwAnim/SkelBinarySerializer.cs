using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class SkelBinarySerializer : IBinarySerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public SkelBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var skel = this.context.Resolve<AnimSkel>();
			skel.NameHash = parser.ConsumeUInt32();

			var numBones = parser.ConsumeUInt32();
			while (numBones > 0)
			{
				parser.Expect(Hash.Get("CIwAnimBone"));
				var nameHash = parser.ConsumeUInt32();
				var parent = parser.ConsumeUInt32();
				var parentIndex = skel.EnsureBone(parent);
				var boneIndex = skel.EnsureBone(nameHash);
				var bone = skel.Bones[boneIndex];
				bone.Parent = parentIndex;
				bone.BindingRot = parser.ConsumeQuaternion();
				bone.BindingPos = parser.ConsumeVector3();
				bone.SkelId = parser.ConsumeUInt16();
				bone.Flags = parser.ConsumeUInt16();

				--numBones;
			}
			return skel;
		}

		#endregion
	}
}
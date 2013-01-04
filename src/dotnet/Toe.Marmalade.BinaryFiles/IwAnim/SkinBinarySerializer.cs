using System;

using Autofac;

using OpenTK;

using Toe.Marmalade.IwAnim;
using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class SkinBinarySerializer : IBinarySerializer
	{
		private readonly IComponentContext context;

		public SkinBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var skin = context.Resolve<AnimSkin>();
			skin.NameHash = parser.ConsumeUInt32();
			skin.Flags = parser.ConsumeUInt32();
			skin.SkeletonModel.HashReference = parser.ConsumeUInt32();
			skin.Skeleton.HashReference = parser.ConsumeUInt32();
			var numGroups = parser.ConsumeUInt32();
			while (numGroups > 0)
			{
				--numGroups;
				var type = parser.ConsumeUInt32();
				if (type == Hash.Get("CIwAnimSkinSet"))
				{
					this.ParseSkinSet(parser, skin);
					continue;
				}
				throw new FormatException();
			}
			return skin;
		}

		private void ParseSkinSet(BinaryParser parser, AnimSkin skin)
		{
			var name = parser.ConsumeUInt32();
			var numVerts = parser.ConsumeUInt32();
			var numBones = parser.ConsumeByte();
			parser.Expect((byte)1);
			var boneIds = parser.ConsumeByteArray(numBones);
			var vertIds = parser.ConsumeUInt16Array((int)numVerts);
			var preMultipliedPositions = parser.ConsumeVector3Array((int)numVerts * numBones);
			var weights = parser.ConsumeFloatArray((int)numVerts * numBones);

			for (int index = 0; index < vertIds.Length; index++)
			{
				var id = vertIds[index];
				skin.Weights.EnsureAt(id);
				var vertexWeights = new VertexWeights { };
				if (numBones > 0)
				{
					vertexWeights.Bone0 = new VertexWeight { BoneIndex = boneIds[0], Weight = weights[0 + index*numBones] };
				}
				if (numBones > 1)
				{
					vertexWeights.Bone1 = new VertexWeight { BoneIndex = boneIds[1], Weight = weights[1 + index * numBones] };
				}
				if (numBones > 2)
				{
					vertexWeights.Bone2 = new VertexWeight { BoneIndex = boneIds[2], Weight = weights[2 + index * numBones] };
				}
				if (numBones > 3)
				{
					vertexWeights.Bone3 = new VertexWeight { BoneIndex = boneIds[3], Weight = weights[3 + index * numBones] };
				}
				skin.Weights[id] = vertexWeights;
			}
		}
	}
}
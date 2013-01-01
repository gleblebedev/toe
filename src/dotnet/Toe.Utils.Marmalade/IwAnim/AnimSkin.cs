using System.Collections.Generic;

using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Utils.Marmalade.IwAnim
{
	public class AnimSkin : Managed
	{
		private readonly IResourceManager resourceManager;

		public AnimSkin(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
			skeletonModel = new ResourceReference(Model.TypeHash, resourceManager, this);
		}

		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkin");

		private readonly MeshStream<VertexWeight> weights = new MeshStream<VertexWeight>();

		private readonly BoneCollection bones = new BoneCollection();

		private ResourceReference skeleton;

		private ResourceReference skeletonModel;

		public BoneCollection Bones
		{
			get
			{
				return this.bones;
			}
		}

		public MeshStream<VertexWeight> Weights
		{
			get
			{
				return this.weights;
			}
		}

		public ResourceReference Skeleton
		{
			get
			{
				return skeleton;
			}
			
		}

		public ResourceReference SkeletonModel
		{
			get
			{
				return skeletonModel;
			}
			
		}

		public int EnsureBone(string boneName)
		{
			return bones.EnsureBone(boneName);
		}

		#endregion

		#region Public Methods and Operators

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		#endregion
	}
}
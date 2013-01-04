using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimSkin : Managed
	{
		private readonly IResourceManager resourceManager;

		public AnimSkin(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			this.skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
			this.skeletonModel = new ResourceReference(Model.TypeHash, resourceManager, this);
		}

		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkin");

		private readonly MeshStream<VertexWeights> weights = new MeshStream<VertexWeights>();

		private readonly BoneCollection bones = new BoneCollection();

		private ResourceReference skeleton;

		private ResourceReference skeletonModel;

		private uint flags;

		public BoneCollection Bones
		{
			get
			{
				return this.bones;
			}
		}

		public MeshStream<VertexWeights> Weights
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
				return this.skeleton;
			}
			
		}

		public ResourceReference SkeletonModel
		{
			get
			{
				return this.skeletonModel;
			}
			
		}

		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureBone(boneName);
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

		public uint Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		#endregion
	}
}
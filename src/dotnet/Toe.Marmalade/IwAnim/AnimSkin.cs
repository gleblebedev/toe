using Toe.Marmalade.IwGraphics;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimSkin : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkin");

		private readonly ManagedList<AnimBone> bones;

		private readonly IResourceManager resourceManager;

		private readonly ResourceReference skeleton;

		private readonly ResourceReference skeletonModel;

		private readonly ListMeshStream<VertexWeights> weights = new ListMeshStream<VertexWeights>();

		#endregion

		#region Constructors and Destructors

		public AnimSkin(IResourceManager resourceManager, ManagedList<AnimBone> b)
		{
			this.bones = b;
			this.resourceManager = resourceManager;
			this.skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
			this.skeletonModel = new ResourceReference(Model.TypeHash, resourceManager, this);
		}

		#endregion

		#region Public Properties

		public ManagedList<AnimBone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public uint Flags { get; set; }

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

		public ListMeshStream<VertexWeights> Weights
		{
			get
			{
				return this.weights;
			}
		}

		#endregion

		#region Public Methods and Operators

		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureItem(boneName);
		}

		#endregion
	}
}
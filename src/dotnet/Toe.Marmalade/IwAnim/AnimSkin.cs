using Toe.Marmalade.IwGraphics;
using Toe.Resources;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimSkin : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkin");

		private readonly BoneCollection bones = new BoneCollection();

		private readonly IResourceManager resourceManager;

		private readonly ResourceReference skeleton;

		private readonly ResourceReference skeletonModel;

		private readonly MeshStream<VertexWeights> weights = new MeshStream<VertexWeights>();

		#endregion

		#region Constructors and Destructors

		public AnimSkin(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			this.skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
			this.skeletonModel = new ResourceReference(Model.TypeHash, resourceManager, this);
		}

		#endregion

		#region Public Properties

		public BoneCollection Bones
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

		public MeshStream<VertexWeights> Weights
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
			return this.bones.EnsureBone(boneName);
		}

		#endregion
	}
}
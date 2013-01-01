using System.Collections.Generic;

using Toe.Resources;

namespace Toe.Utils.Marmalade.IwAnim
{
	public class Anim : Managed
	{
		private readonly IResourceManager resourceManager;

		public Anim(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
		}

		public static readonly uint TypeHash = Hash.Get("CIwAnim");

		private IList<AnimKeyFrame> frames = new List<AnimKeyFrame>();

		private ResourceReference skeleton;

		public ResourceReference Skeleton
		{
			get
			{
				return skeleton;
			}
			
		}

		#region Overrides of Managed

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		#endregion

		public void AddFrame(AnimKeyFrame frame)
		{
			this.frames.Add(frame);
		}

		public void Apply(AnimSkel animSkel, float t)
		{
			var frame = this.frames[0];
			foreach (var bone in frame.Bones)
			{
				var b = animSkel.Bones[animSkel.EnsureBone(bone.Name)];
				b.ActualPos = bone.BindingPos;
				b.ActualRot = bone.BindingRot;
			}
		}
	}
}
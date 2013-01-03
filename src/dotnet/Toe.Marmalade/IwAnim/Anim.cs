using System.Collections.Generic;

using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwAnim;

namespace Toe.Marmalade.IwAnim
{
	public class Anim : Managed
	{
		private readonly IResourceManager resourceManager;

		public Anim(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			this.skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
		}

		public static readonly uint TypeHash = Hash.Get("CIwAnim");

		private IList<AnimKeyFrame> frames = new List<AnimKeyFrame>();

		private ResourceReference skeleton;

		public ResourceReference Skeleton
		{
			get
			{
				return this.skeleton;
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

		public float Duration { get; set; }

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
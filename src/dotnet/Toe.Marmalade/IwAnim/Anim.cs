using System.Collections.Generic;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwAnim
{
	public class Anim : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnim");

		private readonly IList<AnimKeyFrame> frames = new List<AnimKeyFrame>();

		private readonly IResourceManager resourceManager;

		private readonly ResourceReference skeleton;

		#endregion

		#region Constructors and Destructors

		public Anim(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
			this.skeleton = new ResourceReference(AnimSkel.TypeHash, resourceManager, this);
		}

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public float Duration { get; set; }

		public ResourceReference Skeleton
		{
			get
			{
				return this.skeleton;
			}
		}

		#endregion

		#region Properties

		private bool IsAnonymousBones
		{
			get
			{
				if (this.frames.Count == 0)
				{
					return true;
				}
				if (this.frames[0].Bones.Count == 0)
				{
					return true;
				}
				if (this.frames[0].Bones[0].NameHash == 0)
				{
					return true;
				}
				return false;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void AddFrame(AnimKeyFrame frame)
		{
			this.frames.Add(frame);
		}

		public void Apply(AnimSkel animSkel, float t)
		{
			if (this.frames == null)
			{
				return;
			}
			if (this.frames.Count == 0)
			{
				return;
			}
			var frame = this.frames[0];
			if (frame.Bones.Count == 0)
			{
				return;
			}
			if (this.IsAnonymousBones)
			{
				for (int index = 0; index < frame.Bones.Count; ++index)
				{
					var bone = frame.Bones[index];
					AnimBone b;
					if (!this.IsAnonymousBones)
					{
						b = animSkel.Bones[animSkel.EnsureBone(bone.NameHash)];
					}
					else
					{
						b = animSkel.Bones[index];
					}
					b.ActualPos = bone.BindingPos;
					b.ActualRot = bone.BindingRot;
				}
			}
		}

		#endregion
	}
}
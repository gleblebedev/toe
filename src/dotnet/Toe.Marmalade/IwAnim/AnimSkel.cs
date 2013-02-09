using System;

using OpenTK;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwAnim
{
	public class AnimSkel : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkel");

		private readonly ManagedList<AnimBone> bones;

		public AnimSkel( ManagedList<AnimBone> b)
		{
			bones = b;
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

		#endregion

		#region Public Methods and Operators

		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureItem(boneName);
		}

		public int EnsureBone(uint boneName)
		{
			return this.bones.EnsureItem(boneName);
		}

		#endregion

		public void UpdateAbsoluteValues()
		{
			for (int index = 0; index < this.bones.Count; index++)
			{
				this.UpdateBoneAbsoluteValues(index);
			}
		}

		private void UpdateBoneAbsoluteValues(int index)
		{
			var meshBone = this.bones[index];
			var pos = meshBone.ActualPos;
			var rot = meshBone.ActualRot;
			rot = new Quaternion(rot.X, rot.Y, rot.Z, rot.W);
			if (meshBone.Parent >= 0)
			{
				if (index < meshBone.Parent)
				{
					// TODO: this isn't efficient way
					this.UpdateBoneAbsoluteValues(meshBone.Parent);
				}
				var parent = this.bones[meshBone.Parent];
				var ppos = parent.AbsolutePos;
				var prot = parent.AbsoluteRot;

				meshBone.AbsolutePos = Vector3.Transform(pos, prot) + ppos;
				meshBone.AbsoluteRot = Quaternion.Multiply(prot, rot);
			}
			else
			{
				meshBone.AbsolutePos = pos;
				meshBone.AbsoluteRot = rot;
			}
		}
	}
}
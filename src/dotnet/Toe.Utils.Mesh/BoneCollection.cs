using System;
using System.Collections;
using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh
{
	public class BoneCollection : IEnumerable<MeshBone>, IEnumerable
	{
		private readonly List<MeshBone> bones = new List<MeshBone>();

		public int Capacity
		{
			get
			{
				return this.bones.Capacity;
			}
			set
			{
				this.bones.Capacity = value;
			}
		}
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
			rot = new Quaternion(rot.X,rot.Y,rot.Z,rot.W);
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

		public int EnsureBone(string boneName)
		{
			int index;
			for (index = 0; index < this.bones.Count; index++)
			{
				var bone = this.bones[index];
				if (bone.Name == boneName)
				{
					return index;
				}
			}
			this.bones.Add(new MeshBone { Name = boneName });
			return index;
		}

		public MeshBone this[int index]
		{
			get
			{
				return this.bones[index];
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<MeshBone> GetEnumerator()
		{
			return bones.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
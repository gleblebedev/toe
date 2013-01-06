using System;
using System.Collections;
using System.Collections.Generic;

using OpenTK;

using Toe.Resources;

namespace Toe.Utils.Mesh
{
	public class BoneCollection : IEnumerable<MeshBone>, IEnumerable
	{
		#region Constants and Fields

		private readonly List<MeshBone> bones = new List<MeshBone>();

		#endregion

		#region Public Properties

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

		public int Count
		{
			get
			{
				return this.bones.Count;
			}
		}

		#endregion

		#region Public Indexers

		public MeshBone this[int index]
		{
			get
			{
				return this.bones[index];
			}
		}

		#endregion

		#region Public Methods and Operators

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
			UInt32 hash = Hash.Get(boneName);
			for (index = 0; index < this.bones.Count; index++)
			{
				var bone = this.bones[index];
				if (bone.NameHash == hash)
				{
					return index;
				}
			}
			this.bones.Add(new MeshBone { Name = boneName });
			return index;
		}

		public int EnsureBone(uint boneName)
		{
			int index;
			for (index = 0; index < this.bones.Count; index++)
			{
				var bone = this.bones[index];
				if (bone.NameHash == boneName)
				{
					return index;
				}
			}
			this.bones.Add(new MeshBone { NameHash = boneName });
			return index;
		}

		public MeshBone EnsureBoneAt(int index)
		{
			while (index >= this.bones.Count)
			{
				this.bones.Add(new MeshBone());
			}
			return this.bones[index];
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
			return this.bones.GetEnumerator();
		}

		public void UpdateAbsoluteValues()
		{
			for (int index = 0; index < this.bones.Count; index++)
			{
				this.UpdateBoneAbsoluteValues(index);
			}
		}

		#endregion

		#region Explicit Interface Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Methods

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

		#endregion
	}
}
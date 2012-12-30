using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public class BoneCollection
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
	}
}
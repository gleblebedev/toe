using Toe.Resources;
using Toe.Utils.Mesh;
using Toe.Utils.Mesh.Marmalade;

namespace Toe.Utils.Marmalade.IwAnim
{
	public class AnimSkel : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkel");

		private readonly BoneCollection bones = new BoneCollection();

		public BoneCollection Bones
		{
			get
			{
				return this.bones;
			}
		}


		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureBone(boneName);
		}

		#endregion

		#region Public Methods and Operators

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion
	}
}
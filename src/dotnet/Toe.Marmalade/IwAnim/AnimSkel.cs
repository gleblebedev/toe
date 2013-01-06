using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimSkel : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimSkel");

		private readonly BoneCollection bones = new BoneCollection();

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

		#endregion

		#region Public Methods and Operators

		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureBone(boneName);
		}

		public int EnsureBone(uint boneName)
		{
			return this.bones.EnsureBone(boneName);
		}

		#endregion
	}
}
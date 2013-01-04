using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimKeyFrame
	{
		private float time;

		private BoneCollection bones = new BoneCollection();

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public BoneCollection Bones
		{
			get
			{
				return this.bones;
			}
			set
			{
				this.bones = value;
			}
		}
	}
}
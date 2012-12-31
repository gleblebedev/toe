using System.Collections;

using Toe.Utils.Mesh;

namespace Toe.Utils.Marmalade.IwAnim
{
	public class AnimKeyFrame
	{
		private float time;

		private BoneCollection bones = new BoneCollection();

		public float Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}

		public BoneCollection Bones
		{
			get
			{
				return bones;
			}
			set
			{
				bones = value;
			}
		}
	}
}
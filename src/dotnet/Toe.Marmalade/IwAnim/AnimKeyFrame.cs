using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimKeyFrame
	{
		#region Constants and Fields

		private readonly ManagedList<AnimBone> bones;

		AnimKeyFrame(ManagedList<AnimBone> b)
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

		public float Time { get; set; }

		#endregion
	}
}
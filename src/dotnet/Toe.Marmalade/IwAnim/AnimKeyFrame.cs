namespace Toe.Marmalade.IwAnim
{
	public class AnimKeyFrame
	{
		#region Constants and Fields

		private readonly ManagedList<AnimBone> bones;

		#endregion

		#region Constructors and Destructors

		private AnimKeyFrame(ManagedList<AnimBone> b)
		{
			this.bones = b;
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
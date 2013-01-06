using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwAnim
{
	public class AnimKeyFrame
	{
		#region Constants and Fields

		private BoneCollection bones = new BoneCollection();

		#endregion

		#region Public Properties

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

		public float Time { get; set; }

		#endregion
	}
}
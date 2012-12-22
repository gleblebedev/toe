using OpenTK;

namespace Toe.Utils.Mesh
{
	public class MeshBone
	{
		#region Constants and Fields

		private int parent = -1;

		#endregion

		#region Public Properties

		public string Name { get; set; }

		public int Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		public Vector3 Pos { get; set; }

		public Quaternion Rot { get; set; }

		#endregion
	}
}
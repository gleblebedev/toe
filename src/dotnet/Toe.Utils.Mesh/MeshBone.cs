#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else
using OpenTK;

#endif

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

		public Vector3 BindingPos { get; set; }

		public Quaternion BindingRot { get; set; }

		private Vector3? actualPos;

		public Vector3 ActualPos
		{
			get
			{
				if (this.actualPos == null) return BindingPos;
				return this.actualPos.Value;
			}
			set
			{
				this.actualPos = value;
			}
		}

		private Quaternion? actualRot;

		public Quaternion ActualRot
		{
			get
			{
				if (actualRot == null) return BindingRot;
				return this.actualRot.Value;
			}
			set
			{
				this.actualRot = value;
			}
		}

		public Vector3 AbsolutePos { get; set; }

		public Quaternion AbsoluteRot { get; set; }

		#endregion
	}
}
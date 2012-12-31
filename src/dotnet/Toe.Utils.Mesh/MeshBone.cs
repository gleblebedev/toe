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

		private Vector3 absolutePos = Vector3.Zero;

		private Quaternion absoluteRot = Quaternion.Identity;

		private Vector3? actualPos;

		private Quaternion? actualRot;

		private Vector3 bindingPos = Vector3.Zero;

		private Quaternion bindingRot = Quaternion.Identity;

		private int parent = -1;

		#endregion

		#region Public Properties

		public Vector3 AbsolutePos
		{
			get
			{
				return this.absolutePos;
			}
			set
			{
				this.absolutePos = value;
			}
		}

		public Quaternion AbsoluteRot
		{
			get
			{
				return this.absoluteRot;
			}
			set
			{
				this.absoluteRot = value;
			}
		}

		public Vector3 ActualPos
		{
			get
			{
				if (this.actualPos == null)
				{
					return this.BindingPos;
				}
				return this.actualPos.Value;
			}
			set
			{
				this.actualPos = value;
			}
		}

		public Quaternion ActualRot
		{
			get
			{
				if (this.actualRot == null)
				{
					return this.BindingRot;
				}
				return this.actualRot.Value;
			}
			set
			{
				this.actualRot = value;
			}
		}

		public Vector3 BindingPos
		{
			get
			{
				return this.bindingPos;
			}
			set
			{
				this.bindingPos = value;
			}
		}

		public Quaternion BindingRot
		{
			get
			{
				return this.bindingRot;
			}
			set
			{
				this.bindingRot = value;
			}
		}

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

		#endregion
	}
}
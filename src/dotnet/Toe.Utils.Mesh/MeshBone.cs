#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

using OpenTK;

using Toe.Resources;

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

		private string name;

		private uint nameHash;

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

		public ushort Flags { get; set; }

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.nameHash = Hash.Get(value);
				}
			}
		}

		public uint NameHash
		{
			get
			{
				return this.nameHash;
			}
			set
			{
				if (this.nameHash != value)
				{
					this.nameHash = value;
					this.name = null;
				}
			}
		}

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

		public int SkelId { get; set; }

		#endregion
	}
}
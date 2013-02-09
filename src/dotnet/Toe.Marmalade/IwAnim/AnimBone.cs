using OpenTK;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwAnim
{
	public class AnimBone:Managed
	{
		private int parent = -1;

		public static readonly uint TypeHash = Hash.Get("CIwAnimBone");

		private Vector3 bindingPos;

		private Quaternion bindingRot;

		private int skelId;

		private Vector3? actualPos;

		private Quaternion? actualRot;

		private ushort flags;

		private Vector3 absolutePos;

		private Quaternion absoluteRot;

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public int SkelId
		{
			get
			{
				return skelId;
			}
			set
			{
				skelId = value;
			}
		}

		public Vector3 BindingPos
		{
			get
			{
				return bindingPos;
			}
			set
			{
				bindingPos = value;
			}
		}

		public Quaternion BindingRot
		{
			get
			{
				return bindingRot;
			}
			set
			{
				bindingRot = value;
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
				if (this.parent != value)
				{
					this.parent = value;
					this.RaisePropertyChanged("Parent");
				}
			}
		}

		public Vector3 ActualPos
		{
			get
			{
				return actualPos.HasValue?actualPos.Value:bindingPos;
			}
			set
			{
				actualPos = value;
			}
		}

		public Quaternion ActualRot
		{
			get
			{
				return actualRot.HasValue ? actualRot.Value : bindingRot;
			}
			set
			{
				actualRot = value;
			}
		}

		public ushort Flags
		{
			get
			{
				return flags;
			}
			set
			{
				if (flags != value)
				{
					flags = value;
					this.RaisePropertyChanged("Flags");
				}
			}
		}

		public Vector3 AbsolutePos
		{
			get
			{
				return absolutePos;
			}
			set
			{
				absolutePos = value;
			}
		}

		public Quaternion AbsoluteRot
		{
			get
			{
				return absoluteRot;
			}
			set
			{
				absoluteRot = value;
			}
		}
	}
}
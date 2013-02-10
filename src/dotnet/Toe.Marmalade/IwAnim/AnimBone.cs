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

		protected static PropertyEventArgs ParentEventArgs = Expr.PropertyEventArgs<AnimBone>(x => x.Parent);

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
					this.RaisePropertyChanging(ParentEventArgs.Changing);
					this.parent = value;
					this.RaisePropertyChanged(ParentEventArgs.Changed);
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

		protected static PropertyEventArgs FlagsEventArgs = Expr.PropertyEventArgs<AnimBone>(x => x.Flags);

		public ushort Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				if (this.flags != value)
				{
					this.RaisePropertyChanging(FlagsEventArgs.Changing);
					this.flags = value;
					this.RaisePropertyChanged(FlagsEventArgs.Changed);
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
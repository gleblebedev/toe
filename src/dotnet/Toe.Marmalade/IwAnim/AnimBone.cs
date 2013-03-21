using OpenTK;

using Toe.Utils;

namespace Toe.Marmalade.IwAnim
{
	public class AnimBone : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwAnimBone");

		protected static PropertyEventArgs FlagsEventArgs = Expr.PropertyEventArgs<AnimBone>(x => x.Flags);

		protected static PropertyEventArgs ParentEventArgs = Expr.PropertyEventArgs<AnimBone>(x => x.Parent);

		private Vector3? actualPos;

		private Quaternion? actualRot;

		private Vector3 bindingPos;

		private Quaternion bindingRot;

		private ushort flags;

		private int parent = -1;

		#endregion

		#region Public Properties

		public Vector3 AbsolutePos { get; set; }

		public Quaternion AbsoluteRot { get; set; }

		public Vector3 ActualPos
		{
			get
			{
				return this.actualPos.HasValue ? this.actualPos.Value : this.bindingPos;
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
				return this.actualRot.HasValue ? this.actualRot.Value : this.bindingRot;
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

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

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

		public int SkelId { get; set; }

		#endregion
	}
}
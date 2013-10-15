#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

using Toe.Utils.ToeMath;

#endif

namespace Toe.Utils.Mesh
{
	public class MeshBone
	{
		#region Constants and Fields

		private Float3 absolutePos = Float3.Zero;

		private Float4 absoluteRot = Float4.QuaternionIdentity;

		private Float3? actualPos;

		private Float4? actualRot;

		private Float3 bindingPos = Float3.Zero;

		private Float4 bindingRot = Float4.QuaternionIdentity;

		private string name;

		private int parent = -1;

		#endregion

		#region Public Properties

		public Float3 AbsolutePos
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

		public Float4 AbsoluteRot
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

		public Float3 ActualPos
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

		public Float4 ActualRot
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

		public Float3 BindingPos
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

		public Float4 BindingRot
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
using System.Collections.Generic;

using OpenTK;

using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.IwGraphics
{
	public class Model : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwModel");

		protected static PropertyEventArgs FlagsEventArgs = Expr.PropertyEventArgs<Model>(x => x.Flags);

		private readonly IList<Mesh> meshes = new List<Mesh>();

		private readonly IResourceManager resourceManager;

		private uint flags;

		#endregion

		#region Constructors and Destructors

		public Model(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		#endregion

		#region Public Properties

		public Vector3 Center { get; set; }

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public uint Flags
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

		public IList<Mesh> Meshes
		{
			get
			{
				return this.meshes;
			}
		}

		public float Radius { get; set; }

		#endregion
	}
}
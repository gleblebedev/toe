using System;
using System.Collections;
using System.Collections.Generic;

using Toe.Marmalade.IwGraphics.TangentSpace;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class Surface : Managed, IVertexIndexSource
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CSurface");

		private readonly ResourceReference material;

		#endregion

		#region Constructors and Destructors

		public Surface(IResourceManager resourceManager)
		{
			this.material = new ResourceReference(IwGx.Material.TypeHash, resourceManager, this);
		}

		#endregion

		#region Public Properties

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public virtual int Count
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ResourceReference Material
		{
			get
			{
				return this.material;
			}
		}

		public Mesh Mesh { get; set; }

		public virtual VertexSourceType VertexSourceType
		{
			get
			{
				return VertexSourceType.TriangleList;
			}
		}

		public virtual IList<int> GetIndexReader(string key, int channel)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Public Methods and Operators

		#endregion

		#region Explicit Interface Methods

	

		#endregion

		#region Methods

		internal virtual void CalculateTangents(TangentMixer t, TangentMixer b)
		{
			//throw new System.NotImplementedException();
		}

		#endregion
	}
}
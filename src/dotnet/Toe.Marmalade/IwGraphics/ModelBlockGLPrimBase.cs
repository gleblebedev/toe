using System;

using Toe.Marmalade.IwGraphics.TangentSpace;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLPrimBase : Surface
	{
		#region Constants and Fields

		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockGLPrimBase");

		private readonly ListMeshStream<int> indices = new ListMeshStream<int>();

		#endregion

		#region Constructors and Destructors

		public ModelBlockGLPrimBase(IResourceManager resourceManager)
			: base(resourceManager)
		{
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

		public ListMeshStream<int> Indices
		{
			get
			{
				return this.indices;
			}
		}

		#endregion

		#region Methods

		internal override void CalculateTangents(TangentMixer t, TangentMixer b)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
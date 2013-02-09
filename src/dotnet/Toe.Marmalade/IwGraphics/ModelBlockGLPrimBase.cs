using System.Collections.Generic;

using OpenTK;

using Toe.Marmalade.IwGraphics.TangentSpace;
using Toe.Resources;
using Toe.Utils;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockGLPrimBase : Surface
	{
		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockGLPrimBase");

		private readonly MeshStream<int> indices = new MeshStream<int>();

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public MeshStream<int> Indices
		{
			get
			{
				return this.indices;
			}
		}
		internal override void CalculateTangents(TangentMixer t, TangentMixer b)
		{
			throw new System.NotImplementedException();
		}

		public ModelBlockGLPrimBase(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}
	}
}
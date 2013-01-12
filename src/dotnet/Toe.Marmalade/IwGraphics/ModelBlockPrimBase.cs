using System.Collections.Generic;

using OpenTK;

using Toe.Resources;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.IwGraphics
{
	public class ModelBlockPrimBase:Surface
	{
		public static new readonly uint TypeHash = Hash.Get("CIwModelBlockPrimBase");

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		public override IEnumerator<int> GetEnumerator()
		{
			int i = 0;
			foreach (var index in indices)
			{
				yield return i;
				++i;
			}
		}

		private MeshStream<ComplexIndex> indices = new MeshStream<ComplexIndex>();

		public MeshStream<ComplexIndex> Indices
		{
			get
			{
				return this.indices;
			}
			set
			{
				this.indices = value;
			}
		}

		public ModelBlockPrimBase(IResourceManager resourceManager)
			: base(resourceManager)
		{
		}
	}
}
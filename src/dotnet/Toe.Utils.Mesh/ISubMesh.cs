using System.Collections.Generic;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Submesh with single material.
	/// </summary>
	public interface ISubMesh : IVertexIndexSource
	{
		#region Public Properties

		Float3 BoundingBoxMax { get; }

		Float3 BoundingBoxMin { get; }

		Float3 BoundingSphereCenter { get; }

		float BoundingSphereR { get; }

		IMaterial Material { get; set; }

		string Name { get; }

		object RenderData { get; set; }

		#endregion

		#region Public Methods and Operators

		void InvalidateBounds();

		/// <summary>
		/// Get number of indices.
		/// Each stream should have same number of indices.
		/// </summary>
		int Count { get; }

		IList<int> GetIndexReader(string key, int channel);

		#endregion
	}
}
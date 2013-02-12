using System.Collections.Generic;

using OpenTK;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Visible surface determination provider.
	/// </summary>
	public interface IVsdProvider
	{
		/// <summary>
		/// Main mesh, similar to the level geometry.
		/// </summary>
		IMesh Level { get; }

		/// <summary>
		/// Camera position.
		/// </summary>
		Vector3 CameraPosition { get; set; }

		IEnumerable<ISubMesh> GetVisibleSubMeshes();
	}
}
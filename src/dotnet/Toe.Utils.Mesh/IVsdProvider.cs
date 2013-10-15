using System.Collections.Generic;

using Toe.Utils.ToeMath;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Visible surface determination provider.
	/// </summary>
	public interface IVsdProvider
	{
		#region Public Properties

		/// <summary>
		/// Camera position.
		/// </summary>
		Float3 CameraPosition { get; set; }

		/// <summary>
		/// Main mesh, similar to the level geometry.
		/// </summary>
		IMesh Level { get; }

		#endregion

		#region Public Methods and Operators

		IEnumerable<ISubMesh> GetVisibleSubMeshes();

		#endregion
	}
}
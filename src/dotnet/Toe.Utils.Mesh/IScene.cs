using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic scene conainer interface.
	/// </summary>
	public interface IScene : ISceneItem, INodeContainer
	{
		#region Public Properties

		IList<IEffect> Effects { get; }

		IList<IMesh> Geometries { get; }

		IList<IImage> Images { get; }

		IList<IMaterial> Materials { get; }

		/// <summary>
		/// Visible surface determination provider.
		/// </summary>
		IVsdProvider VsdProvider { get; set; }

		#endregion
	}
}
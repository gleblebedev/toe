using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Mesh as set of submeshes.
	/// </summary>
	public interface IMesh
	{
		IList<ISubMesh> Submeshes { get; }

		string Name { get; }

#if WINDOWS_PHONE
#else
		void RenderOpenGL();
#endif
	}
}
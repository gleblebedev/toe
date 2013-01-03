namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Submesh with single material.
	/// </summary>
	public interface ISubMesh
	{
#if WINDOWS_PHONE
#else
		void RenderOpenGL();
#endif

		string Material { get; set; }

		string Name { get; }

		uint MaterialHash { get; set; }
	}
}
namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	/// <summary>
	/// Depth write mode. Only affects gl.
	/// @see GetDepthWrite, SetDepthWrite
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	enum DepthWriteMode
	{
		DEPTH_WRITE_NORMAL,             //!< Defer to blend mode to determine depth buffer write
		DEPTH_WRITE_DISABLED,           //!< Disable depth buffer write
	};
}
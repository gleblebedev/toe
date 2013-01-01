namespace Toe.Utils.Marmalade.IwGx
{
	/// <summary>
	/// Depth write mode. Only affects gl.
	/// @see GetDepthWrite, SetDepthWrite
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	internal enum DepthWriteMode
	{
		DEPTH_WRITE_NORMAL, //!< Defer to blend mode to determine depth buffer write
		DEPTH_WRITE_DISABLED, //!< Disable depth buffer write
	};
}
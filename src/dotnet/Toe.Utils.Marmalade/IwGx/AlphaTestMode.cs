namespace Toe.Utils.Mesh.Marmalade.IwGx
{
	/// <summary>
	/// Alpha (transparency) Testing modes.
	/// @see GetAlphaTestMode, SetAlphaTestMode
	/// @par Required Header Files
	/// IwMaterial.h
	/// </summary>
	public enum AlphaTestMode
	{
		DISABLED, //!< no alpha testing;
		NEVER, //!< never pass alpha test;
		LESS, //!< pass alpha test when alpha is less that reference value;
		EQUAL, //!< pass alpha test when alpha is equal that reference value;
		LEQUAL, //!< pass alpha test when alpha is less or equal that reference value;
		GREATER, //!< pass alpha test when alpha is greater that reference value;
		NOTEQUAL, //!< pass alpha test when alpha is not equal that reference value;
		GEQUAL, //!< pass alpha test when alpha is greater or equal that reference value;
		ALWAYS //!< always pass the alpha test;
	};
}
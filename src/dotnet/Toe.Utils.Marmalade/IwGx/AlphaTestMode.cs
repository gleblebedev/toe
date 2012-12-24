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
		ALPHATEST_DISABLED,     //!< no alpha testing;
		ALPHATEST_NEVER,        //!< never pass alpha test;
		ALPHATEST_LESS,         //!< pass alpha test when alpha is less that reference value;
		ALPHATEST_EQUAL,        //!< pass alpha test when alpha is equal that reference value;
		ALPHATEST_LEQUAL,       //!< pass alpha test when alpha is less or equal that reference value;
		ALPHATEST_GREATER,      //!< pass alpha test when alpha is greater that reference value;
		ALPHATEST_NOTEQUAL,     //!< pass alpha test when alpha is not equal that reference value;
		ALPHATEST_GEQUAL,       //!< pass alpha test when alpha is greater or equal that reference value;
		ALPHATEST_ALWAYS        //!< always pass the alpha test;
	};
}
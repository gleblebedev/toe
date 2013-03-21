namespace Toe.Utils.Mesh
{
	public interface ISceneReaderErrorHandler
	{
		#region Public Methods and Operators

		/// <summary>
		/// Error.
		/// If Error doesn't throw an exception scene reading should proceed.
		/// </summary>
		/// <param name="msg">Error message.</param>
		void Error(string msg);

		/// <summary>
		/// Warning.
		/// </summary>
		/// <param name="msg">Message.</param>
		void Warning(string msg);

		#endregion
	}
}
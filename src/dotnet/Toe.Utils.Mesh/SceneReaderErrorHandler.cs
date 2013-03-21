using System.Diagnostics;

namespace Toe.Utils.Mesh
{
	public class SceneReaderErrorHandler : ISceneReaderErrorHandler
	{
		#region Public Methods and Operators

		/// <summary>
		/// Error.
		/// If Error doesn't throw an exception scene reading should proceed.
		/// </summary>
		/// <param name="msg">Error message.</param>
		public void Error(string msg)
		{
			throw new SceneReaderException(msg);
		}

		/// <summary>
		/// Warning.
		/// </summary>
		/// <param name="msg">Message.</param>
		public void Warning(string msg)
		{
			Trace.WriteLine(string.Format("Warning: {0}", msg));
		}

		#endregion
	}
}
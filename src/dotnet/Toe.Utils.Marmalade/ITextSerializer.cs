using Toe.Utils.Mesh.Marmalade;

namespace Toe.Utils.Marmalade
{
	public interface ITextSerializer
	{
		#region Public Properties

		string DefaultFileExtension { get; }

		#endregion

		#region Public Methods and Operators

		Managed Parse(TextParser parser);

		#endregion
	}
}
namespace Toe.Utils.Mesh
{
	public interface IMaterialBinding
	{
		#region Public Properties

		IMaterial Material { get; set; }

		string Target { get; }

		#endregion
	}
}
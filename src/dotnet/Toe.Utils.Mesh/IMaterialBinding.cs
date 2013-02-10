namespace Toe.Utils.Mesh
{
	public interface IMaterialBinding
	{
		string Target { get; }
		IMaterial Material { get; set; }
	}
}
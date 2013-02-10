namespace Toe.Utils.Mesh
{
	public interface INodeSkin
	{
		IMaterialBinding this[string taraget] { get; set; }

		void RemoveBinding(string target);
	}
}
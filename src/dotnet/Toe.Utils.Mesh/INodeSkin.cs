namespace Toe.Utils.Mesh
{
	public interface INodeSkin
	{
		#region Public Indexers

		IMaterialBinding this[string taraget] { get; set; }

		#endregion

		#region Public Methods and Operators

		void RemoveBinding(string target);

		#endregion
	}
}
namespace Toe.Core
{
	public enum ToeComponentLayerPopularity
	{
		/// <summary>
		/// Almost every entity have this component.
		/// </summary>
		VeryPopular,

		/// <summary>
		/// Big number of entities have this component.
		/// </summary>
		Popular,

		/// <summary>
		/// Some entities have this component.
		/// </summary>
		Average,

		/// <summary>
		/// Small number of entities have this component.
		/// </summary>
		Rare,

		/// <summary>
		/// Less than 10 entities have this component. 
		/// </summary>
		VeryRare
	}
}
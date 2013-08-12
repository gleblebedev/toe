namespace Toe.Messaging.Types
{
	public class VectorXYBinarySerializer : StructSerializer<Vector2>, ITypeBinarySerializer
	{
		#region Constructors and Destructors

		public VectorXYBinarySerializer()
			: base(PropertyTypes.VectorXY, a => a.X, a => a.Y)
		{
		}

		#endregion
	}
}
namespace Toe.Messaging.Types
{
	public class VectorXYZWBinarySerializer : StructSerializer<Vector4>, ITypeBinarySerializer
	{
		#region Constructors and Destructors

		public VectorXYZWBinarySerializer()
			: base(PropertyTypes.VectorXYZW, a => a.X, a => a.Y, a => a.Z, a => a.W)
		{
		}

		#endregion
	}
}
namespace Toe.Messaging.Types
{
	public class VectorXYZBinarySerializer : StructSerializer<Vector3>, ITypeBinarySerializer
	{
		#region Constructors and Destructors

		public VectorXYZBinarySerializer()
			: base(PropertyTypes.VectorXYZ, a => a.X, a => a.Y, a => a.Z)
		{
		}

		#endregion
	}
}
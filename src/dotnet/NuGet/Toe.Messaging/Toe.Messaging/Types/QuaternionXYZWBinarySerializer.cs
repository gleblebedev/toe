namespace Toe.Messaging.Types
{
	public class QuaternionXYZWBinarySerializer : StructSerializer<Quaternion>, ITypeBinarySerializer
	{
		#region Constructors and Destructors

		public QuaternionXYZWBinarySerializer()
			: base(PropertyTypes.QuaternionXYZW, a => a.X, a => a.Y, a => a.Z, a => a.W)
		{
		}

		#endregion
	}
}
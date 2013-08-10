using Toe.Messaging.Types;
using Quaternion = OpenTK.Quaternion;
namespace Toe.Messaging.OpenTK
{
	public class QuaternionXYZWBinarySerializer : StructSerializer<Quaternion>, ITypeBinarySerializer
	{
		public QuaternionXYZWBinarySerializer()
			: base(PropertyTypes.QuaternionXYZW, a => a.X, a => a.Y, a => a.Z, a => a.W)
		{
		}
	}
}
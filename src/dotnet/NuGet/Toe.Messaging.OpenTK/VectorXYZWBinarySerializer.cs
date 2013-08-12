using Toe.Messaging.Types;

using Vector4 = OpenTK.Vector4;
namespace Toe.Messaging.OpenTK
{
	public class VectorXYZWBinarySerializer : StructSerializer<Vector4>, ITypeBinarySerializer
	{
		public VectorXYZWBinarySerializer(): base(PropertyTypes.VectorXYZW, a => a.X, a => a.Y, a => a.Z, a => a.W)
		{
		}
	}
}
using Toe.Messaging.Types;
using Vector2 = OpenTK.Vector2;
namespace Toe.Messaging.OpenTK
{
	public class VectorXYBinarySerializer : StructSerializer<Vector2>, ITypeBinarySerializer
	{
		public VectorXYBinarySerializer(): base(PropertyTypes.VectorXY, a => a.X, a => a.Y)
		{
		}
	}
}
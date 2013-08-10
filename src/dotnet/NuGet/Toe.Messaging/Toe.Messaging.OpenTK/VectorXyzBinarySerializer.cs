using System;
using System.Linq.Expressions;

using Toe.Messaging.Types;

using Vector3 = OpenTK.Vector3;
namespace Toe.Messaging.OpenTK
{
	public class VectorXYZBinarySerializer : StructSerializer<Vector3>, ITypeBinarySerializer
	{
		public VectorXYZBinarySerializer(): base(PropertyTypes.VectorXYZ, a => a.X, a => a.Y, a => a.Z)
		{
		}
	}
}
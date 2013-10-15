using System.Reflection;

namespace Toe.Utils.Mesh
{
	public class VertexBufferChannel : IVertexBufferChannel
	{
		public VertexBufferChannel(PropertyInfo propertyInfo)
			: this((MemberInfo)propertyInfo)
		{
			
		}

		public VertexBufferChannel(FieldInfo propertyInfo)
			: this((MemberInfo)propertyInfo)
		{
			
		}

		private VertexBufferChannel(MemberInfo propertyInfo)
		{

		}
	}
}
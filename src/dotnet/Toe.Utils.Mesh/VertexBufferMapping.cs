using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Toe.Utils.Mesh
{
	public class VertexBufferMapping<TVertex>
	{
		static VertexBufferMapping<TVertex> defaultInstance;
		public static VertexBufferMapping<TVertex> Default
		{
			get
			{
				return defaultInstance;
			}
		}

		static VertexBufferMapping()
		{
			defaultInstance = new VertexBufferMapping<TVertex>(DiscoverChannelsViaReflection());
		}

		private static IEnumerable<IVertexBufferChannel> DiscoverChannelsViaReflection()
		{
			var type = typeof(TVertex);
			foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				yield return new VertexBufferChannel(propertyInfo);
			}
			foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
			{
				yield return new VertexBufferChannel(fieldInfo);
			}
		}

		public VertexBufferMapping(IEnumerable<IVertexBufferChannel> channels)
		{

		}
	
	}
}
using System.Collections.Generic;

using Toe.Messaging.AluminumLua.Types;

namespace Toe.Messaging.AluminumLua
{
	public class LuaTypeRegistry : Registry<ILuaTypeSerializer>
	{
		public LuaTypeRegistry(IEnumerable<ILuaTypeSerializer> serializers):base(serializers,x=>x.PropertyType)
		{
			
		}
		#region Public Methods and Operators

		public static LuaTypeRegistry CreateDefault()
		{
			return new LuaTypeRegistry(new ILuaTypeSerializer[]
				{
					new Int32LuaTypeSerializer(), 
					new SingleLuaTypeSerializer(), 
					new StringLuaTypeSerializer(), 
				});
		}

		#endregion

		public ILuaTypeSerializer GetSerializer(int propertyType)
		{
			return base.BinarySearch(propertyType);
		}
	}
}
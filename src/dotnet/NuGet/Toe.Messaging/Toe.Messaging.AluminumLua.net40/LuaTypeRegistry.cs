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

	    public static IEnumerable<ILuaTypeSerializer> StandartTypes
	    {
	        get
	        {
	            return new ILuaTypeSerializer[]
	                { new Int32LuaTypeSerializer(), new SingleLuaTypeSerializer(), new StringLuaTypeSerializer(), };
	        }
	    }

	    public static LuaTypeRegistry CreateDefault()
		{
            return new LuaTypeRegistry(StandartTypes);
		}

		#endregion

		public ILuaTypeSerializer GetSerializer(int propertyType)
		{
			return base.BinarySearch(propertyType);
		}
	}
}
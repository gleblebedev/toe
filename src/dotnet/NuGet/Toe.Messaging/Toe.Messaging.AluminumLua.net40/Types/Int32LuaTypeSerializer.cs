using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public class Int32LuaTypeSerializer:ILuaTypeSerializer
	{
		#region Implementation of ILuaTypeSerializer

		public int PropertyType { get
		{
			return Messaging.PropertyType.Int32;
		} }

		public int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue)
		{
			return 0;
		}

		#endregion
	}
}
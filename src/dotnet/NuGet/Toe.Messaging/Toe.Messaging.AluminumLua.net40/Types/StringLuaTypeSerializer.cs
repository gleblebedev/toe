using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public class StringLuaTypeSerializer : ILuaTypeSerializer
	{
		#region Implementation of ILuaTypeSerializer

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyType.String;
			}
		}

		public int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue)
		{
			if (propertyValue.IsNil)
				return queue.GetStringLength(null);
			return queue.GetStringLength(propertyValue.AsString());
		}

		#endregion
	}
}
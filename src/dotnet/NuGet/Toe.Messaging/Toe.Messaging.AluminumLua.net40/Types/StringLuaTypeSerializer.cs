using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public class StringLuaTypeSerializer : ILuaTypeSerializer
	{
		#region Public Properties

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyTypes.String;
			}
		}

		#endregion

		#region Public Methods and Operators

		public int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue)
		{
			if (propertyValue.IsNil)
			{
				return queue.GetStringLength(null);
			}
			return queue.GetStringLength(propertyValue.AsString());
		}

		#endregion
	}
}
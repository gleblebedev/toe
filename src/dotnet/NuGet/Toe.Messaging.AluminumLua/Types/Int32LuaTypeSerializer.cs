using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public class Int32LuaTypeSerializer : ILuaTypeSerializer
	{
		#region Public Properties

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyTypes.Int32;
			}
		}

		#endregion

		#region Public Methods and Operators

		public int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue)
		{
			return 0;
		}

		#endregion
	}
}
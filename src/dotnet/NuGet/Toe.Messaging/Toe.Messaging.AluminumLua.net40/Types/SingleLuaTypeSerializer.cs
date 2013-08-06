using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public class SingleLuaTypeSerializer : ILuaTypeSerializer
	{
		#region Public Properties

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyType.Single;
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
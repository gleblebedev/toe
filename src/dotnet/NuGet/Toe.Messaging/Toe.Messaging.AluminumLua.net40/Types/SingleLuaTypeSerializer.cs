using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public class SingleLuaTypeSerializer : ILuaTypeSerializer
	{
		#region Implementation of ILuaTypeSerializer

		public int PropertyType
		{
			get
			{
				return Messaging.PropertyType.Single;
			}
		}

		public int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue)
		{
			return 0;
		}

		#endregion
	}
}
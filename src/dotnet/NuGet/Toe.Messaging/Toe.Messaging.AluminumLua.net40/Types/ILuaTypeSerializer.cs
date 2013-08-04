using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public interface ILuaTypeSerializer
	{
		int PropertyType { get; }

		int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue);
	}
}
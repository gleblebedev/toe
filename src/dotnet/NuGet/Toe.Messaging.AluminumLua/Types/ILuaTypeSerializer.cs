using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua.Types
{
	public interface ILuaTypeSerializer
	{
		#region Public Properties

		int PropertyType { get; }

		#endregion

		#region Public Methods and Operators

		int GetDynamicSize(IMessageQueue queue, LuaObject propertyValue);

		#endregion
	}
}
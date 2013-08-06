using System.Collections.Generic;

using Toe.Messaging.AluminumLua.Types;

namespace Toe.Messaging.AluminumLua
{
	public class LuaTypeRegistry : Registry<ILuaTypeSerializer>
	{
		#region Constructors and Destructors

		public LuaTypeRegistry(IEnumerable<ILuaTypeSerializer> serializers)
			: base(serializers, x => x.PropertyType)
		{
		}

		#endregion

		#region Public Properties

		public static IEnumerable<ILuaTypeSerializer> StandartTypes
		{
			get
			{
				return new ILuaTypeSerializer[]
					{ new Int32LuaTypeSerializer(), new SingleLuaTypeSerializer(), new StringLuaTypeSerializer(), };
			}
		}

		#endregion

		#region Public Methods and Operators

		public static LuaTypeRegistry CreateDefault()
		{
			return new LuaTypeRegistry(StandartTypes);
		}

		public ILuaTypeSerializer GetSerializer(int propertyType)
		{
			return base.BinarySearch(propertyType);
		}

		#endregion
	}
}
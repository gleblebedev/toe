﻿using System;
using System.Collections.Generic;

using AluminumLua;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.AluminumLua
{
	public class LuaSerializer
	{
		#region Constants and Fields

		private readonly MessageRegistry registry;

		private readonly LuaTypeRegistry luaTypeRegistry;

		#endregion

		#region Constructors and Destructors

		public LuaSerializer(MessageRegistry registry, LuaTypeRegistry luaTypeRegistry)
		{
			this.registry = registry;
			this.luaTypeRegistry = luaTypeRegistry;
		}

		#endregion

		#region Public Methods and Operators

		public LuaObject Receive(IMessageQueue queue, int position)
		{
			var messageId = queue.ReadInt32(position);
			var message = this.registry.GetDefinition(messageId);
			Dictionary<LuaObject, LuaObject> table = new Dictionary<LuaObject, LuaObject>();
			foreach (var property in message.Properties)
			{
				var key = LuaObject.FromString(property.Name);
				switch (property.PropertyType)
				{
					case PropertyType.Int32:
						table[key] = LuaObject.FromNumber(queue.ReadInt32(position+property.Offset));
						break;
					case PropertyType.Single:
						table[key] = LuaObject.FromNumber(queue.ReadFloat(position + property.Offset));
						break;
					default:
						throw new NotImplementedException();
				}
			}
			return LuaObject.FromTable(table);
		}

		public void Send(IMessageQueue queue, LuaObject luaObject)
		{
			var id = (int)luaObject[LuaObject.FromString("MessageId")].AsNumber();
			var message = this.registry.GetDefinition(id);
			var position = queue.Allocate(message.MinSize);
			foreach (var keyValue in luaObject.AsTable())
			{
				var keyObject = keyValue.Key;
				int propertyKey = 0;
				if (keyObject.IsNumber)
				{
					propertyKey = (int)keyObject.AsNumber();
				}
				else
				{
					propertyKey = Hash.Eval(keyObject.AsString());
				}

				var property = message.GetPropertyById(propertyKey);
				switch (property.PropertyType)
				{
					case PropertyType.Int32:
						queue.WriteInt32(position+property.Offset, (int)keyValue.Value.AsNumber());
						break;
					case PropertyType.Single:
						queue.WriteFloat(position + property.Offset, (float)keyValue.Value.AsNumber());
						break;
					default:
						throw new NotImplementedException();
				}
			}
			queue.Commit(position);
		}

		#endregion
	}
}
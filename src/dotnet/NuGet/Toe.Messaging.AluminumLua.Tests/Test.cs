using System.Collections.Generic;

using AluminumLua;

using NUnit.Framework;

using Toe.CircularArrayQueue;
using Toe.Messaging.Types;

namespace Toe.Messaging.AluminumLua.Tests
{
	[TestFixture]
	public class Test
	{
		#region Public Methods and Operators

		[Test]
		public void TestTable()
		{
			var reg = new MessageRegistry();
			var a = new LuaSerializer(reg, LuaTypeRegistry.CreateDefault());
			var ser = new DefaultSerializer<SubMessage>(reg, TypeRegistry.CreateDefault());

			using (var buf = new ThreadSafeWriteQueue(1024))
			{
				var dictionary = new Dictionary<LuaObject, LuaObject> { };
				dictionary.Add(LuaObject.FromString("MessageId"), LuaObject.FromNumber(ser.MessageId));
				var table = LuaObject.FromTable(dictionary);
				table[LuaObject.FromString("Text")] = LuaObject.FromString("abc");
				a.Send(buf, table);

				int pos = buf.ReadMessage();
				var id = buf.ReadInt32(pos);
				Assert.AreEqual(ser.MessageId, id);

				var table2 = a.Receive(buf, pos);
				Assert.AreEqual(ser.MessageId, table2[LuaObject.FromString("MessageId")].AsNumber());
				Assert.AreEqual(table[LuaObject.FromString("Text")].AsString(), table2[LuaObject.FromString("Text")].AsString());
			}
		}

		#endregion
	}
}
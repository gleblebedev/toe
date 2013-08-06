using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using AluminumLua;

using NUnit.Framework;

using OpenTK;

using Toe.CircularArrayQueue;
using Toe.Messaging.AluminumLua;
using Toe.Messaging.AluminumLua.Types;
using Toe.Messaging.Types;

namespace Toe.Messaging.OpenTK.Tests
{
    [TestFixture]
    public class Test
    {
        #region Public Methods and Operators

        [Test]
        public void BinarySerilization()
        {
            var reg = new MessageRegistry();
            var typeReg = new TypeRegistry(new[] { TypeRegistry.StandartTypes, new[] { new VectorXyzBinarySerializer() } }.SelectMany(x => x));
            var ser = new DefaultSerializer<SubMessage>(reg, typeReg);

            using (var buf = new ThreadSafeWriteQueue(1024))
            {
                var src = new SubMessage() { Vector3 = new Vector3(1.1f, 2.2f, 3.3f) };
                ser.Serialize(buf,src);
                var dst = new SubMessage();
                ser.Deserialize(buf,buf.ReadMessage(),dst);
                Assert.AreEqual(src.Vector3, dst.Vector3);
            }
        }

        [Test]
        public void LuaSerilization()
        {
            var reg = new MessageRegistry();
            var luaReg = new LuaTypeRegistry(new[] { LuaTypeRegistry.StandartTypes }.SelectMany(x => x));
            var a = new LuaSerializer(reg, luaReg);
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
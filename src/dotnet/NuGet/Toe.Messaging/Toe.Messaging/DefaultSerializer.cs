using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Toe.CircularArrayQueue;
using System.Threading;

namespace Toe.Messaging
{
	public class DefaultSerializer<T> : IMessageSerializer<T>
	{
		private int messageId;

		private BinarySerializerContext reusableContext = new BinarySerializerContext();

		private Action<IMessageQueue, T> serialize;

	    private Action<IMessageQueue, int, T> deserialize;

	    private void ReleaseContext(BinarySerializerContext context)
		{
			Interlocked.CompareExchange(ref reusableContext, context, null);
		}
		private BinarySerializerContext AllocateContext()
		{
			BinarySerializerContext context;
			do
			{
				context = reusableContext;
				if (context == null) return new BinarySerializerContext();
			}
			while (context != Interlocked.CompareExchange(ref reusableContext, null, context));
			return context;
		}


	    public DefaultSerializer(MessageRegistry messageRegistry, int messageId)
        {
            this.messageId = messageId;
            var description = messageRegistry.DefineMessage(this.messageId);

            var parameter = Expression.Parameter(typeof(T), "value");

            var allMembers = GetMembers(typeof(T));

            var fixedSize = 0;
            var allocateContext = ((Func<BinarySerializerContext>)AllocateContext).Method;
            var releaseContext = ((Action<BinarySerializerContext>)ReleaseContext).Method;
            var allocate =
                ((Func<IMessageQueue, int, int, BinarySerializerContext>)(new BinarySerializerContext()).Allocate).Method;
            var writeInt32 = ((Func<int, int, BinarySerializerContext>)(new BinarySerializerContext()).WriteInt32).Method;
            var commit = ((Func<BinarySerializerContext>)reusableContext.Commit).Method;
            var queueParameter = Expression.Parameter(typeof(IMessageQueue), "q");
            var positionParameter = Expression.Parameter(typeof(int), "p");
            var messageParameter = Expression.Parameter(typeof(T), "m");
            var thisExpression = Expression.Constant(this);
            {
                var chainElement = Expression.Call(thisExpression, allocateContext);
                chainElement = Expression.Call(
                    chainElement, allocate, queueParameter, Expression.Constant(fixedSize), Expression.Constant(0));
                foreach (var member in allMembers)
                {
                    switch (member.Type)
                    {
                        case PropertyType.Int32:
                            {
                                Expression src = Expression.PropertyOrField(messageParameter, member.MemberInfo.Name);
                                if (member.PropertyType != typeof(int)) src = Expression.Convert(src, typeof(int));
                                chainElement = Expression.Call(
                                    chainElement, writeInt32, Expression.Constant(member.Offset), src);
                            }
                            break;
                    }
                }
                chainElement = Expression.Call(chainElement, commit);
                var body = Expression.Call(thisExpression, releaseContext, chainElement);
                var serializeExpression = Expression.Lambda<Action<IMessageQueue, T>>(body, queueParameter, messageParameter);
                this.serialize = serializeExpression.Compile();
            }
            {
                //var chainElement = Expression.Call(thisExpression, allocateContext);

                //var deserializeExpression = Expression.Lambda<Action<IMessageQueue, int, T>>(
                //    body, queueParameter, positionParameter, messageParameter);
                //this.deserialize = deserializeExpression.Compile();
            }
        }

	    private List<MessageMemberInfo> GetMembers(Type type)
	    {
	        if (type == typeof(object))
                return new List<MessageMemberInfo>();
	        var baseList = this.GetMembers(type.BaseType);
            baseList.AddRange(this.GetTypeMembers(type));
	        return baseList;
	    }

	 
        private List<MessageMemberInfo> GetTypeMembers(Type t)
	    {
	        var all =
	            t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
	                     .Select(x=>new MessageMemberInfo(x))
                         .Where(a => a.Type != PropertyType.Unknown)
	                     .ToList();
	        all.AddRange(
	            t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
	                      .Select(x=>new MessageMemberInfo(x))
                          .Where(a => a.Type != PropertyType.Unknown)
	                     .ToList());
            all.Sort((a, b) =>
                {
                    if (a.Order < b.Order) return -1;
                    if (a.Order > b.Order) return 1;
                    return string.Compare(a.Name, b.Name);
                });
	        return all;
	    }

	    #region Implementation of IMessageSerializer

		void IMessageSerializer.Serialize(IMessageQueue queue, object value)
		{
			this.Serialize(queue,(T)value);
		}

		void IMessageSerializer.Deserialize(IMessageQueue queue, int pos, object value)
		{
			this.Deserialize(queue, pos, (T)value);
		}

		#endregion

		#region Implementation of IMessageSerializer<T>

		public void Serialize(IMessageQueue queue, T value)
		{
			serialize(queue, value);
		}

		public void Deserialize(IMessageQueue queue,int pos, T value)
		{
		    this.deserialize(queue, pos, value);
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public class StringBinarySerializer : ITypeBinarySerializer
	{
		#region Implementation of ITypeBinarySerializer

		public int FixedSize { get
		{
			return 1;
		} }

		public static readonly ITypeBinarySerializer Instance = new StringBinarySerializer();

		public Expression BuildDeserializeExpression(MessageMemberInfo member, Expression positionParameter, Expression queue, ParameterExpression messageParameter)
		{
			var body =
				Expression.Assign(member.GetProperty(messageParameter),
				Expression.Call(((Func<IMessageQueue, int, string>)FetchString).Method, queue, 
				Expression.Add(positionParameter,
				Expression.Call(queue, MessageQueueMethods.ReadInt32, Expression.Add(Expression.Constant(member.Offset), positionParameter))
				))
				);
			return Expression.Block(new ParameterExpression[] { }, new Expression[]
				{
					MessageQueueMethods.TraceWriteLine(positionParameter),
					MessageQueueMethods.TraceWriteLine(Expression.Add(Expression.Constant(member.Offset), positionParameter)),
					MessageQueueMethods.TraceWriteLine(Expression.Call(queue, MessageQueueMethods.ReadInt32, Expression.Add(Expression.Constant(member.Offset), positionParameter))),
					body,
				});
		}

		internal static int EvaluateSize(string str)
		{
			return (Encoding.UTF8.GetByteCount(str)+4)>>2;
		}
		internal static string FetchString(IMessageQueue queue, int position)
		{
			var res = new List<byte>(32);
			for (;;)
			{
				int val = queue.ReadInt32(position);
				++position;

				byte b;
				b = (byte)(val & 255);
				if (b == 0)break;
				res.Add(b);
				b = (byte)((val >> 8) & 255);
				if (b == 0) break;
				res.Add(b);
				b = (byte)((val >> 16) & 255);
				if (b == 0) break;
				res.Add(b);
				b = (byte)((val >> 24) & 255);
				if (b == 0) break;
				res.Add(b);
			}
			return Encoding.UTF8.GetString(res.ToArray());
		}
		internal static int CopyString(IMessageQueue queue, int dynamic, string str)
		{
			str = str ?? String.Empty;
			var bytes = Encoding.UTF8.GetBytes(str);
			int index = 0;
			for (; index + 3 < bytes.Length; index += 4)
			{
				queue.WriteInt32(
					dynamic, ((int)bytes[index]) | ((int)bytes[index + 1] << 8) | (bytes[index + 2] << 16) | (bytes[index + 3] << 24));
				++dynamic;
			}
			int val = 0;
			if (index < bytes.Length)
			{
				val |= ((int)bytes[index]);
				++index;
				if (index < bytes.Length)
				{
					val |= ((int)bytes[index]) << 8;
					++index;
					if (index < bytes.Length)
					{
						val |= ((int)bytes[index]) << 16;
					}
				}
			}
			queue.WriteInt32(dynamic, val);
			++dynamic;
			return dynamic;
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, ParameterExpression messageParameter)
		{
			return Expression.Call(((Func<string, int>)EvaluateSize).Method, member.GetProperty(messageParameter));
		}

		public Expression BuildSerializeExpression(MessageMemberInfo member, Expression positionParameter, Expression dynamicPositionParameter, Expression queue, ParameterExpression messageParameter)
		{
			//Expression expression = member.GetProperty(messageParameter);
			//if (member.PropertyType != typeof(int))
			//{
			//	expression = Expression.Convert(expression, typeof(int));
			//}
			//return Expression.Call(
			//	queue,
			//	MessageQueueMethods.WriteInt32,
			//	Expression.Add(positionParameter, Expression.Constant(member.Offset)),
			//	expression);

			return Expression.Block(
				new ParameterExpression[] { },
				new Expression[]
					{
						Expression.Call(
							queue,
							MessageQueueMethods.WriteInt32,
							Expression.Add(positionParameter, Expression.Constant(member.Offset)),
							Expression.Subtract(dynamicPositionParameter, positionParameter)), 
							MessageQueueMethods.TraceWriteLine(positionParameter),
						Expression.Assign(dynamicPositionParameter, Expression.Call(
								((Func<IMessageQueue, int, string,int>)CopyString).Method, queue, dynamicPositionParameter, member.GetProperty(messageParameter)))
					});
		}

		#endregion
	}
}
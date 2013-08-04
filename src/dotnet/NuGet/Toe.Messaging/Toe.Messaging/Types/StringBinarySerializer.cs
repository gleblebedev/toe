using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using Toe.CircularArrayQueue;

namespace Toe.Messaging.Types
{
	public class StringBinarySerializer : ITypeBinarySerializer
	{
		#region Constants and Fields

		public static readonly ITypeBinarySerializer Instance = new StringBinarySerializer();

		#endregion

		#region Public Properties

		public int FixedSize
		{
			get
			{
				return 1;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void BuildDeserializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			var body = Expression.Assign(
				member.GetProperty(context.MessageParameter),
				Expression.Call(
					((Func<IMessageQueue, int, string>)FetchString).Method,
					context.QueueParameter,
					Expression.Add(
						context.PositionParameter,
						Expression.Call(
							context.QueueParameter,
							MessageQueueMethods.ReadInt32,
							Expression.Add(Expression.Constant(member.Offset), context.PositionParameter)))));
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.ReadInt32,
					Expression.Add(Expression.Constant(member.Offset), context.PositionParameter)));
			context.Code.Add(body);
		}

		public Expression BuildDynamicSizeEvaluator(MessageMemberInfo member, BinarySerilizationContext context)
		{
			return Expression.Call(((Func<string, int>)EvaluateSize).Method, member.GetProperty(context.MessageParameter));
		}

		public void BuildSerializeExpression(MessageMemberInfo member, BinarySerilizationContext context)
		{
			context.Code.Add(
				Expression.Call(
					context.QueueParameter,
					MessageQueueMethods.WriteInt32,
					Expression.Add(context.PositionParameter, Expression.Constant(member.Offset)),
					Expression.Subtract(context.DynamicPositionParameter, context.PositionParameter)));
			context.Code.Add(
				Expression.Assign(
					context.DynamicPositionParameter,
					Expression.Call(
						((Func<IMessageQueue, int, string, int>)CopyString).Method,
						context.QueueParameter,
						context.DynamicPositionParameter,
						member.GetProperty(context.MessageParameter))));
		}

		#endregion

		#region Methods

		internal static int CopyString(IMessageQueue queue, int dynamic, string str)
		{
			str = str ?? String.Empty;
			var bytes = Encoding.UTF8.GetBytes(str);
			int index = 0;
			for (; index + 3 < bytes.Length; index += 4)
			{
				queue.WriteInt32(
					dynamic, (bytes[index]) | (bytes[index + 1] << 8) | (bytes[index + 2] << 16) | (bytes[index + 3] << 24));
				++dynamic;
			}
			int val = 0;
			if (index < bytes.Length)
			{
				val |= (bytes[index]);
				++index;
				if (index < bytes.Length)
				{
					val |= (bytes[index]) << 8;
					++index;
					if (index < bytes.Length)
					{
						val |= (bytes[index]) << 16;
					}
				}
			}
			queue.WriteInt32(dynamic, val);
			++dynamic;
			return dynamic;
		}

		internal static int EvaluateSize(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 1;
			}
			return (Encoding.UTF8.GetByteCount(str) + 4) >> 2;
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
				if (b == 0)
				{
					break;
				}
				res.Add(b);
				b = (byte)((val >> 8) & 255);
				if (b == 0)
				{
					break;
				}
				res.Add(b);
				b = (byte)((val >> 16) & 255);
				if (b == 0)
				{
					break;
				}
				res.Add(b);
				b = (byte)((val >> 24) & 255);
				if (b == 0)
				{
					break;
				}
				res.Add(b);
			}
			var array = res.ToArray();
			return Encoding.UTF8.GetString(array,0,array.Length);
		}

		#endregion
	}
}
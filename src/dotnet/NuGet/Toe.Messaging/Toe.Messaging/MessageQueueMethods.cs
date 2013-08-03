using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

using Toe.CircularArrayQueue;

namespace Toe.Messaging
{
	public class MessageQueueMethods
	{
		#region Constants and Fields

		private static MethodInfo allocate;

		private static MethodInfo commit;

		private static MethodInfo readFloat;

		private static MethodInfo readInt32;

		private static MethodInfo writeFloat;

		private static MethodInfo writeInt32;

		private static MethodInfo writeLine;

		#endregion

		#region Public Properties

		public static MethodInfo Allocate
		{
			get
			{
				return allocate ?? (allocate = typeof(IMessageQueue).GetMethod("Allocate"));
			}
		}

		public static MethodInfo Commit
		{
			get
			{
				return commit ?? (commit = typeof(IMessageQueue).GetMethod("Commit"));
			}
		}

		public static MethodInfo ReadFloat
		{
			get
			{
				return readFloat ?? (readFloat = typeof(IMessageQueue).GetMethod("ReadFloat"));
			}
		}

		public static MethodInfo ReadInt32
		{
			get
			{
				return readInt32 ?? (readInt32 = typeof(IMessageQueue).GetMethod("ReadInt32"));
			}
		}

		public static MethodInfo WriteFloat
		{
			get
			{
				return writeFloat ?? (writeFloat = typeof(IMessageQueue).GetMethod("WriteFloat"));
			}
		}

		public static MethodInfo WriteInt32
		{
			get
			{
				return writeInt32 ?? (writeInt32 = typeof(IMessageQueue).GetMethod("WriteInt32"));
			}
		}

		public static MethodInfo WriteLine
		{
			get
			{

				return writeLine ?? (writeLine = typeof(Trace).GetMethod("WriteLine", new Type[] { typeof(object) }));
			}
		}
		#endregion

		public static Expression TraceWriteLine(Expression positionParameter)
		{
			
			return Expression.Call(WriteLine, 
				Expression.Call(((Func<string,object,object,string>)string.Format).Method,
				Expression.Constant("{0} = {1}"),
				Expression.Constant((object)(positionParameter.ToString())),
				Expression.Convert(positionParameter, typeof(object))));
		}
	}
}
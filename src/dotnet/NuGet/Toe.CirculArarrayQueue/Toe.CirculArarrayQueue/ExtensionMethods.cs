using System;

namespace Toe.CircularArrayQueue
{
	public static class ExtensionMethods
	{
		#region Public Methods and Operators

		public static int GetStringLength(this IMessageQueue messageQueue, string text)
		{
			int counter = 0;
			foreach (var c in text)
			{
				++counter;
				if (c >= 0x80)
				{
					++counter;
				}
				if (c >= 0x800)
				{
					++counter;
				}
				if (c >= 0x10000)
				{
					++counter;
				}
				if (c >= 0x200000)
				{
					++counter;
				}
				if (c >= 0x4000000)
				{
					++counter;
				}

				//if (c < 0x80) {++counter; continue;}
				//if (c < 0x800) { counter+=2; continue; }
				//if (c < 0x10000) { counter += 3; continue; }
				//if (c < 0x200000) { counter += 4; continue; }
				//if (c < 0x4000000) { counter += 5; continue; }
				//counter += 6;
			}
			return (counter + 4) >> 2;
		}

		public static long ReadInt64(this IMessageQueue messageQueue, int position)
		{
#if DEBUG
			if (messageQueue == null)
			{
				throw new ArgumentNullException("messageQueue");
			}
#endif
			var a = (long)(uint)messageQueue.ReadInt32(position);
			var b = (long)(uint)messageQueue.ReadInt32(position + 1);
			return b << 32 | a;
		}

		public static void WriteInt64(this IMessageQueue messageQueue, int position, long value)
		{
#if DEBUG
			if (messageQueue == null)
			{
				throw new ArgumentNullException("messageQueue");
			}
#endif
			messageQueue.WriteInt32(position, (int)value);
			messageQueue.WriteInt32(position + 1, (int)(value >> 32));
		}

		public static int WriteString(this IMessageQueue messageQueue, int position, string text)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
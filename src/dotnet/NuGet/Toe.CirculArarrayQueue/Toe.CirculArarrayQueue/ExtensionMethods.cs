using System;
using System.Collections.Generic;
using System.Text;

namespace Toe.CircularArrayQueue
{
	/// <summary>
	/// Extension methods for IMessageQueue.
	/// </summary>
	public static class ExtensionMethods
	{
		#region Public Methods and Operators

		public static int GetStringLength(this IMessageQueue messageQueue, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return 1;
			}
			return (Encoding.UTF8.GetByteCount(text) + 4) >> 2;

			//int counter = 0;
			//foreach (var c in text)
			//{
			//	++counter;
			//	if (c >= 0x80)
			//	{
			//		++counter;
			//	}
			//	if (c >= 0x800)
			//	{
			//		++counter;
			//	}
			//	if (c >= 0x10000)
			//	{
			//		++counter;
			//	}
			//	if (c >= 0x200000)
			//	{
			//		++counter;
			//	}
			//	if (c >= 0x4000000)
			//	{
			//		++counter;
			//	}

			//	//if (c < 0x80) {++counter; continue;}
			//	//if (c < 0x800) { counter+=2; continue; }
			//	//if (c < 0x10000) { counter += 3; continue; }
			//	//if (c < 0x200000) { counter += 4; continue; }
			//	//if (c < 0x4000000) { counter += 5; continue; }
			//	//counter += 6;
			//}
			//return (counter + 4) >> 2;
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
		public static string ReadStringContent(IMessageQueue queue, int contentIndex)
		{
			var res = new List<byte>(32);
			for (; ; )
			{
				int val = queue.ReadInt32(contentIndex);
				++contentIndex;

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
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		public static int WriteStringContent(this IMessageQueue messageQueue, int contentIndex, string str)
		{
			str = str ?? String.Empty;
			var bytes = Encoding.UTF8.GetBytes(str);
			int index = 0;
			for (; index + 3 < bytes.Length; index += 4)
			{
				messageQueue.WriteInt32(
					contentIndex, (bytes[index]) | (bytes[index + 1] << 8) | (bytes[index + 2] << 16) | (bytes[index + 3] << 24));
				++contentIndex;
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
			messageQueue.WriteInt32(contentIndex, val);
			++contentIndex;
			return contentIndex;
		}

		#endregion
	}
}
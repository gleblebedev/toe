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

		/// <summary>
		/// Copy message to other message queue.
		/// </summary>
		/// <param name="messageQueue">Soutce queue.</param>
		/// <param name="position">Position at source queue. Should be valid postion returned by   messageQueue.ReadMessage()</param>
		/// <param name="destinationQueue">Destination queue.</param>
		public static void CopyTo(this IMessageQueue messageQueue, int position, IMessageQueue destinationQueue)
		{
			var len = messageQueue.GetSize(position);
			var targetPosition = destinationQueue.Allocate(len);
			for (int i = 0; i < len; ++i)
			{
				destinationQueue.WriteInt32(targetPosition + i, messageQueue.ReadInt32(i + position));
			}
			destinationQueue.Commit(len);
		}

		public static int GetByteCount(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
            
			int length = 0;
		    for (int index = 0; index < str.Length; index++)
		    {
		        var currentChar = str[index];
		        if (currentChar == 0)
		        {
		            throw new ArgumentException("String can not contain \\0 character");
		        }
		        if (currentChar < '\x80')
		        {
		            ++length;
		        }
		        else if (currentChar < '\x800')
		        {
		            length += 2;
		        }
		        else if (currentChar < '\uD800' || currentChar > '\uDFFF')
		        {
		            length += 3;
		        }
		        else if (currentChar <= '\uDBFF')
		        {
                    if (index + 1 < str.Length && str[index + 1] >= '\uDC00' && str[index + 1] <= '\uDFFF')
                    {
                        ++index;
                        length += 4;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
		        }
		        else
		        {
		            throw new NotImplementedException();
		        }
		    }
		    return length;
		}
		/// <summary>
		/// Calculates the number of 32-bit integers produced by encoding the characters in the specified <see cref="T:System.String"/>.
		/// </summary>
		/// <param name="text">The <see cref="T:System.String"/> containing the set of characters to encode.</param>
		/// <returns>The number of 32-bit integers produced by encoding the specified characters.</returns>
		public static int GetStringLength(this IMessageQueue messageQueue, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return 1;
			}
			return (GetByteCount(text) + 4) >> 2;
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

		public static string ReadStringContent(IMessageQueue queue, int contentIndex)
		{
			//TODO: avoid list allocation.
			var res = new List<byte>(32);
			for (;;)
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

		public static int WriteStringContent(this IMessageQueue messageQueue, int contentIndex, string str)
		{
			str = str ?? String.Empty;
		    int buffer = 0;
		    int shift = 0;
            for (int index = 0; index < str.Length; ++index)
            {
                int ch = str[index];
                if (ch < '\x80')
                {
                    AddByte((byte)ch, ref buffer, ref shift, ref contentIndex, messageQueue);
					} else if (ch < '\x800') {
                        AddByte((byte) (0xC0 | (ch >> 6)), ref buffer, ref shift, ref contentIndex, messageQueue);
                        AddByte((byte) (0x80 | (ch & 0x3F)), ref buffer, ref shift, ref contentIndex, messageQueue);
                    }
                else if (ch < '\uD800' || ch > '\uDFFF')
                {
                    AddByte((byte)(0xE0 | (ch >> 12)), ref buffer, ref shift, ref contentIndex, messageQueue);
                    AddByte((byte)(0x80 | ((ch >> 6) & 0x3F)), ref buffer, ref shift, ref contentIndex, messageQueue);
                    AddByte((byte)(0x80 | (ch & 0x3F)), ref buffer, ref shift, ref contentIndex, messageQueue);
                }
                else if (ch <= '\uDBFF')
                {
                    if (index + 1 < str.Length && str[index + 1] >= '\uDC00' && str[index + 1] <= '\uDFFF')
                    {
                        ch = 0x10000 + (int)str[index + 1] - 0xDC00 + (((int)str[index] - 0xD800) << 10);
                        AddByte((byte)(0xF0 | (ch >> 18)), ref buffer, ref shift, ref contentIndex, messageQueue);
                        AddByte((byte)(0x80 | ((ch >> 12) & 0x3F)), ref buffer, ref shift, ref contentIndex, messageQueue);
                        AddByte((byte)(0x80 | ((ch >> 6) & 0x3F)), ref buffer, ref shift, ref contentIndex, messageQueue);
                        AddByte((byte)(0x80 | (ch & 0x3F)), ref buffer, ref shift, ref contentIndex, messageQueue);
                        ++index;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            AddByte(0, ref buffer, ref shift, ref contentIndex, messageQueue);
            if (shift != 0)
                messageQueue.WriteInt32(contentIndex, buffer);
            return contentIndex + 1;

            ////TODO: avoid array allocation.
            //var bytes = Encoding.UTF8.GetBytes(str);
            //int index = 0;
            //for (; index + 3 < bytes.Length; index += 4)
            //{
            //    messageQueue.WriteInt32(
            //        contentIndex, (bytes[index]) | (bytes[index + 1] << 8) | (bytes[index + 2] << 16) | (bytes[index + 3] << 24));
            //    ++contentIndex;
            //}
            //int val = 0;
            //if (index < bytes.Length)
            //{
            //    val |= (bytes[index]);
            //    ++index;
            //    if (index < bytes.Length)
            //    {
            //        val |= (bytes[index]) << 8;
            //        ++index;
            //        if (index < bytes.Length)
            //        {
            //            val |= (bytes[index]) << 16;
            //        }
            //    }
            //}
            //messageQueue.WriteInt32(contentIndex, val);
            //++contentIndex;
            //return contentIndex;
		}

	    private static void AddByte(byte ch, ref int buffer, ref int shift, ref int contentIndex, IMessageQueue messageQueue)
	    {
	        buffer |= ((int)ch) << shift;
	        shift += 8;
            if (shift == 32)
            {
                messageQueue.WriteInt32(contentIndex,buffer);
                shift = 0;
                buffer = 0;
                ++contentIndex;
            }
	    }

	    #endregion
	}
}
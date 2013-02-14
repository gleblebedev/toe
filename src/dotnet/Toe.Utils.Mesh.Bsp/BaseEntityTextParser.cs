using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

using Toe.Utils.TextParser;

namespace Toe.Utils.Mesh.Bsp
{
	public class BaseEntityTextParser : BaseTextParser, IEnumerable<BspEntity>
	{
		private TextReader reader;

		private readonly Func<string, string, object> convertor;

		private int nextChar;

		readonly StringBuilder sb = new StringBuilder();

		private int line = 0;

		private int pos = 0;

		public BaseEntityTextParser(TextReader reader, Func<string,string,object> convertor)
		{
			this.reader = reader;
			this.convertor = convertor;
			this.nextChar = reader.Read();
		}
		protected override string GetSourceName()
		{
			return string.Format("BSP entities line:{0} pos:{1}",this.line, this.pos);
		}
		protected int ReadNextChar()
		{
			this.nextChar = this.reader.Read();
			if (this.nextChar == '\n')
			{
				++line;
				pos = 0;
			}
			else if (this.nextChar == '\r')
			{
				++pos;
			}
			return this.nextChar;
		}
		protected override void ReadLexem()
		{
			while (this.nextChar >= 0 && char.IsWhiteSpace((char)this.nextChar))
			{
				this.ReadNextChar();
			}
			if (this.nextChar < 0)
			{
				this.Lexem = null;
				return;
			}
			this.sb.Clear();
			if (this.nextChar == '\"')
			{
				var term = this.nextChar;
				this.ReadNextChar();
				this.sb.Clear();
				while (this.nextChar >= 0 && this.nextChar != term)
				{
					this.sb.Append((char)this.nextChar);
					this.ReadNextChar();
				}
				this.ReadNextChar();
				this.Lexem = sb.ToString();
				return;
			}
			while (this.nextChar >= 0 && !char.IsWhiteSpace((char)this.nextChar))
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
			}
			this.Lexem = sb.ToString();
		}

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<BspEntity> GetEnumerator()
		{
			while (Lexem != null)
			{
				var e = new BspEntity();
				this.Consume("{");
				for (; ; )
				{
					if (Lexem == "}")
					{
						this.Consume();
						break;
					}
					var key = this.Consume();
					if (Lexem == "}")
					{
						this.Consume();
						break;
					}
					var value = this.Consume();
					e[key] = ConvertEntityProperty(key, value);
				}
				yield return e;
			}
		}

		protected virtual object ConvertEntityProperty(string key, string value)
		{
			if (convertor != null) return convertor(key, value);
			return value;
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Toe.Utils.TextParser;

namespace Toe.Utils.Mesh.Bsp
{
	public class BaseEntityTextParser : BaseTextParser, IEnumerable<BspEntity>
	{
		#region Constants and Fields

		private readonly Func<string, string, object> convertor;

		private readonly TextReader reader;

		private readonly StringBuilder sb = new StringBuilder();

		private int line;

		private int nextChar;

		private int pos;

		#endregion

		#region Constructors and Destructors

		public BaseEntityTextParser(TextReader reader, Func<string, string, object> convertor)
		{
			this.reader = reader;
			this.convertor = convertor;
			this.nextChar = reader.Read();
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<BspEntity> GetEnumerator()
		{
			while (this.Lexem != null)
			{
				var e = new BspEntity();
				this.Consume("{");
				for (;;)
				{
					if (this.Lexem == "}")
					{
						this.Consume();
						break;
					}
					var key = this.Consume();
					if (this.Lexem == "}")
					{
						this.Consume();
						break;
					}
					var value = this.Consume();
					e[key] = this.ConvertEntityProperty(key, value);
				}
				yield return e;
			}
		}

		#endregion

		#region Explicit Interface Methods

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

		#region Methods

		protected virtual object ConvertEntityProperty(string key, string value)
		{
			if (this.convertor != null)
			{
				return this.convertor(key, value);
			}
			return value;
		}

		protected override string GetSourceName()
		{
			return string.Format("BSP entities line:{0} pos:{1}", this.line, this.pos);
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
				this.Lexem = this.sb.ToString();
				return;
			}
			while (this.nextChar >= 0 && !char.IsWhiteSpace((char)this.nextChar))
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
			}
			this.Lexem = this.sb.ToString();
		}

		protected int ReadNextChar()
		{
			this.nextChar = this.reader.Read();
			if (this.nextChar == '\n')
			{
				++this.line;
				this.pos = 0;
			}
			else if (this.nextChar == '\r')
			{
				++this.pos;
			}
			return this.nextChar;
		}

		#endregion
	}
}
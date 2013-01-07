using System;
using System.Globalization;
using System.IO;

namespace Toe.Utils.TextParser
{
	public class BaseTextParser
	{
		#region Constants and Fields

		private bool isLexemReady;

		private string lexem;

		#endregion

		#region Public Properties

		/// <summary>
		/// Is lexem available.
		/// </summary>
		public bool IsLexemReady
		{
			get
			{
				return this.isLexemReady;
			}
		}

		/// <summary>
		/// Current lexem or null if source stream is ended.
		/// When previous lexem is consumed new one will be parsed automaticly.
		/// </summary>
		public string Lexem
		{
			get
			{
				if (this.IsLexemReady)
				{
					return this.lexem;
				}
				this.ReadLexem();
				return this.lexem;
			}
			protected set
			{
				this.isLexemReady = true;
				this.lexem = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Mark current lexem as consumed.
		/// </summary>
		public string Consume()
		{
			var l = this.Lexem;
			this.isLexemReady = false;
			return l;
		}
		public void Error(string message)
		{
			throw new TextParserException(message);
		}

		public void UnknownLexemError()
		{
			var @where = this.GetSourceName();
			throw new TextParserException(
				string.Format(CultureInfo.InvariantCulture, "Unknown element \"{0}\" in {1}", this.Lexem, where));
		}

		protected virtual string GetSourceName()
		{
			return "input stream";
		}

		public string Consume(string text)
		{
			return this.Consume(text, StringComparison.InvariantCulture);
		}
		public string Consume(string text, StringComparison stringComparison)
		{
			string lexem = this.Lexem;
			if (0 != string.Compare(lexem, text, stringComparison))
			{
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Expected \"{0}\", but there was \"{1}\"", text, lexem));
			}
			this.Consume();
			return lexem;
		}

		public bool ConsumeBool()
		{
			var lexem = this.Lexem;
			if (0 == string.Compare(lexem, "true", StringComparison.InvariantCultureIgnoreCase))
			{
				this.Consume();
				return true;
			}
			if (0 == string.Compare(lexem, "false", StringComparison.InvariantCultureIgnoreCase))
			{
				this.Consume();
				return false;
			}
			return this.ConsumeInt() != 0;
		}

		public byte ConsumeByte()
		{
			var f = byte.Parse(this.Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		/// <summary>
		/// Consume enumeration value.
		/// </summary>
		public T ConsumeEnum<T>() where T : struct
		{
			T res;
			if (!Enum.TryParse(this.Lexem, false, out res))
			{
				if (!Enum.TryParse(this.Lexem, true, out res))
				{
					throw new TextParserException(string.Format("Not valid {0} enum value {1}", typeof(T).Name, this.Lexem));
				}
			}
			this.Consume();
			return res;
		}

		/// <summary>
		/// Consume floating point number.
		/// </summary>
		/// <returns>Floating point number</returns>
		public float ConsumeFloat()
		{
			return this.ConsumeFloat(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Consume floating point number.
		/// </summary>
		/// <returns>Floating point number</returns>
		public float ConsumeFloat(IFormatProvider format)
		{
			var f = float.Parse(this.Lexem, format);
			this.Consume();
			return f;
		}

		/// <summary>
		/// Consume integer number.
		/// </summary>
		public int ConsumeInt()
		{
			return this.ConsumeInt(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Consume integer number.
		/// </summary>
		public int ConsumeInt(IFormatProvider format)
		{
			var f = int.Parse(this.Lexem, format);
			this.Consume();
			return f;
		}

		public short ConsumeShort()
		{
			var f = short.Parse(this.Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public ushort ConsumeUShort()
		{
			var f = ushort.Parse(this.Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public void Skip(string s)
		{
			if (this.Lexem == s)
			{
				this.Consume();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Main parser method.
		/// </summary>
		protected virtual void ReadLexem()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
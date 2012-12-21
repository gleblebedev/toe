using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextParser
	{
		#region Constants and Fields

		private readonly TextReader reader;

		private bool lexemReady = false;

		private string lexem;

		private int nextChar = -1;
		#endregion

		#region Constructors and Destructors

		public TextParser(TextReader reader)
		{
			this.reader = reader;
			nextChar = reader.Read();
		}

		#endregion

		public void Consume(string text)
		{
			if (0!=string.Compare(Lexem,text))
				throw new TextParserException(string.Format("Expected \"{0}\", but there was \"{1}\"", text, this.Lexem));
			this.Consume();
		}
		public void Consume()
		{
			lexemReady = false;
		}
		public string Lexem
		{
			get
			{
				if (lexemReady) return lexem;
				ReadLexem();
				return lexem;
			}
		}

		private void ReadLexem()
		{
			while (nextChar >= 0 && char.IsWhiteSpace((char)nextChar))
			{
				nextChar = reader.Read();
			}
			switch (nextChar)
			{
				case -1:
					lexem = null;
					lexemReady = true;
					return;
				case '{':
					this.OnTerminalSymbol("{");
					return;
				case '}':
					this.OnTerminalSymbol("}");
					return;
				case '(':
					this.OnTerminalSymbol("(");
					return;
				case ')':
					this.OnTerminalSymbol(")");
					return;
				case '=':
					this.OnTerminalSymbol("=");
					return;
				case ',':
					this.OnTerminalSymbol(",");
					return;
				case ';':
					this.OnTerminalSymbol(";");
					return;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					ReadNumber();
					return;
				case '"':
				case '\'':
					ReadString();
					return;
				default:
					ReadId();
					return;
			}
		}

		private void OnTerminalSymbol(string val)
		{
			this.lexem = val;
			this.lexemReady = true;
			this.nextChar = this.reader.Read();
		}

		private void ReadNumber()
		{
			sb.Clear();
			while (nextChar >= 0 && nextChar >= '0' && nextChar <= '9')
			{
				sb.Append((char)nextChar);
				nextChar = reader.Read();
			}
			if (nextChar == '.')
			{
				sb.Append((char)nextChar);
				while (nextChar >= 0 && nextChar >= '0' && nextChar <= '9')
				{
					sb.Append((char)nextChar);
					nextChar = reader.Read();
				}
			}
			lexem = sb.ToString();
			lexemReady = true;
		}

		readonly StringBuilder sb = new StringBuilder(32);

		private void ReadString()
		{
			var term = nextChar;
			nextChar = reader.Read();
			sb.Clear();
			while (nextChar >= 0 && nextChar != term)
			{
				if (nextChar == '\\')
				{
					nextChar = reader.Read();
					switch (nextChar)
					{
						default:
							sb.Append((char)nextChar);
							break;
					}
				}
				else
				{
					sb.Append((char)nextChar);
				}
				nextChar = reader.Read();
			}
			this.OnTerminalSymbol(sb.ToString());
		}

		private void ReadId()
		{
			sb.Clear();
			while (nextChar >= 0 && !char.IsWhiteSpace((char)nextChar) && nextChar != '{' && nextChar != '}' && nextChar != '(' && nextChar != ')' && nextChar != '=' && nextChar != ',' && nextChar != ';')
			{
				sb.Append((char)nextChar);
				nextChar = reader.Read();
			}
			lexem = sb.ToString();
			lexemReady = true;
		}

		public string ConsumeString()
		{
			var l = Lexem;
			//TODO: check if it is string
			this.Consume();
			return l;
		}

		public float ConsumeFloat()
		{
			var f = float.Parse(Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}
	}
}
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

using OpenTK;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh.Marmalade
{
	public class TextParser
	{
		#region Constants and Fields

		private readonly float[] floatBuf = new float[4];

		private readonly TextReader reader;

		private readonly StringBuilder sb = new StringBuilder(32);

		private string lexem;

		private bool lexemReady;

		private int nextChar = -1;

		#endregion

		#region Constructors and Destructors

		public TextParser(TextReader reader)
		{
			this.reader = reader;
			this.nextChar = reader.Read();
		}

		#endregion

		#region Public Properties

		public string GetLexem()
		{
			if (this.lexemReady)
			{
				return this.lexem;
			}
			this.ReadLexem();
			return this.lexem;
		}

		#endregion

		#region Public Methods and Operators

		public void Consume(string text)
		{
			if (0 != string.Compare(this.GetLexem(), text, StringComparison.InvariantCulture))
			{
				throw new TextParserException(string.Format(CultureInfo.InvariantCulture, "Expected \"{0}\", but there was \"{1}\"", text, this.GetLexem()));
			}
			this.Consume();
		}

		public void Consume()
		{
			this.lexemReady = false;
		}

		public Color ConsumeColor()
		{
			int maxItems = 4;
			maxItems = this.ConsumeVector(maxItems);
			if (maxItems == 4)
			{
				return Color.FromArgb(
					ClampColor(this.floatBuf[0]),
					ClampColor(this.floatBuf[1]),
					ClampColor(this.floatBuf[2]),
					ClampColor(this.floatBuf[3]));
			}
			throw new NotImplementedException();
		}

		public float ConsumeFloat()
		{
			var f = float.Parse(this.GetLexem(), CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public int ConsumeInt()
		{
			var f = int.Parse(this.GetLexem(), CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public Quaternion ConsumeQuaternion()
		{
			int maxItems = 4;
			this.ConsumeVector(maxItems);
			return new Quaternion(this.floatBuf[3], this.floatBuf[0], this.floatBuf[1], this.floatBuf[2]);
		}

		public string ConsumeString()
		{
			var l = this.GetLexem();
			//TODO: check if it is string
			this.Consume();
			return l;
		}

		public Vector2 ConsumeVector2()
		{
			int maxItems = 2;
			this.ConsumeVector(maxItems);
			return new Vector2(this.floatBuf[0], this.floatBuf[1]);
		}

		public Vector3 ConsumeVector3()
		{
			int maxItems = 3;
			this.ConsumeVector(maxItems);
			return new Vector3(this.floatBuf[0], this.floatBuf[1], this.floatBuf[2]);
		}

		public void Skip(string s)
		{
			if (this.GetLexem() == s)
			{
				this.Consume();
			}
		}

		#endregion

		#region Methods

		private static int ClampColor(float f)
		{
			return Math.Max(0, Math.Min(255, (int)(f * 255)));
		}

		private int ConsumeVector(int maxItems)
		{
			this.Consume("{");
			int index;
			for (index = 0; index < this.floatBuf.Length; index++)
			{
				this.floatBuf[index] = 0;
			}
			for (index = 0; index < maxItems; ++index)
			{
				var l = this.GetLexem();
				if (l == "}")
				{
					this.Consume();
					return index;
				}
				this.floatBuf[index] = this.ConsumeFloat();
				if (this.GetLexem() == ",")
				{
					this.Consume();
				}
			}
			this.Consume("}");
			return index;
		}

		private void OnTerminalSymbol(string val)
		{
			this.lexem = val;
			this.lexemReady = true;
			this.nextChar = this.reader.Read();
		}

		private void ReadId()
		{
			this.sb.Clear();
			while (this.nextChar >= 0 && !char.IsWhiteSpace((char)this.nextChar) && this.nextChar != '{' && this.nextChar != '}'
			       && this.nextChar != '(' && this.nextChar != ')' && this.nextChar != '=' && this.nextChar != ','
			       && this.nextChar != ';')
			{
				this.sb.Append((char)this.nextChar);
				this.nextChar = this.reader.Read();
			}
			this.lexem = this.sb.ToString();
			this.lexemReady = true;
		}

		private void ReadLexem()
		{
			while (this.nextChar >= 0 && char.IsWhiteSpace((char)this.nextChar))
			{
				this.nextChar = this.reader.Read();
			}
			switch (this.nextChar)
			{
				case -1:
					this.lexem = null;
					this.lexemReady = true;
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
					this.ReadNumber();
					return;
				case '"':
				case '\'':
					this.ReadString();
					return;
				default:
					this.ReadId();
					return;
			}
		}

		private void ReadNumber()
		{
			this.sb.Clear();
			while (this.nextChar >= 0 && this.nextChar >= '0' && this.nextChar <= '9')
			{
				this.sb.Append((char)this.nextChar);
				this.nextChar = this.reader.Read();
			}
			if (this.nextChar == '.')
			{
				this.sb.Append((char)this.nextChar);
				this.nextChar = this.reader.Read();
				while (this.nextChar >= 0 && this.nextChar >= '0' && this.nextChar <= '9')
				{
					this.sb.Append((char)this.nextChar);
					this.nextChar = this.reader.Read();
				}
			}
			if (this.nextChar == 'E')
			{
				this.sb.Append((char)this.nextChar);
				this.nextChar = this.reader.Read();
				if (this.nextChar == '-' || this.nextChar == '+')
				{
					this.sb.Append((char)this.nextChar);
					this.nextChar = this.reader.Read();
				}
				while (this.nextChar >= 0 && this.nextChar >= '0' && this.nextChar <= '9')
				{
					this.sb.Append((char)this.nextChar);
					this.nextChar = this.reader.Read();
				}
			}
			this.lexem = this.sb.ToString();
			this.lexemReady = true;
		}

		private void ReadString()
		{
			var term = this.nextChar;
			this.nextChar = this.reader.Read();
			this.sb.Clear();
			while (this.nextChar >= 0 && this.nextChar != term)
			{
				if (this.nextChar == '\\')
				{
					this.nextChar = this.reader.Read();
					switch (this.nextChar)
					{
						default:
							this.sb.Append((char)this.nextChar);
							break;
					}
				}
				else
				{
					this.sb.Append((char)this.nextChar);
				}
				this.nextChar = this.reader.Read();
			}
			this.OnTerminalSymbol(this.sb.ToString());
		}

		#endregion
	}
}
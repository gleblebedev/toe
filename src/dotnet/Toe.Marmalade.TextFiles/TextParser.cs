using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

using OpenTK;

using Toe.Resources;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Marmalade
{
	public class TextParser
	{
		#region Constants and Fields

		private readonly string basePath;

		private readonly float[] floatBuf = new float[4];

		private readonly TextReader reader;

		private readonly IResourceFile resourceFile;

		private readonly StringBuilder sb = new StringBuilder(32);

		private string lexem;

		private bool lexemReady;

		private int nextChar = -1;

		#endregion

		#region Constructors and Destructors

		public TextParser(TextReader reader, IResourceFile resourceFile, string basePath)
		{
			this.reader = reader;
			this.resourceFile = resourceFile;
			this.basePath = basePath;
			this.nextChar = reader.Read();
		}

		#endregion

		#region Public Properties

		public string BasePath
		{
			get
			{
				return this.basePath;
			}
		}

		public string Lexem
		{
			get
			{
				if (this.lexemReady)
				{
					return this.lexem;
				}
				this.ReadLexem();
				return this.lexem;
			}
		}

		public IResourceFile ResourceFile
		{
			get
			{
				return this.resourceFile;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Consume(string text)
		{
			if (0 != string.Compare(this.Lexem, text, StringComparison.InvariantCulture))
			{
				throw new TextParserException(
					string.Format(CultureInfo.InvariantCulture, "Expected \"{0}\", but there was \"{1}\"", text, this.Lexem));
			}
			this.Consume();
		}

		public void Consume()
		{
			this.lexemReady = false;
		}

		public string ConsumeBlock()
		{
			this.Consume("{");
			int depth = 1;
			this.sb.Clear();
			if (this.lexemReady)
			{
				this.sb.Append(this.lexem);
			}
			for (;;)
			{
				if (this.nextChar < 0)
				{
					this.lexem = this.sb.ToString();
					this.lexemReady = false;
					return this.lexem;
				}
				if (this.nextChar == '{')
				{
					++depth;
				}
				if (this.nextChar == '}')
				{
					--depth;
					if (depth == 0)
					{
						this.nextChar = this.reader.Read();
						this.lexem = this.sb.ToString();
						this.lexemReady = false;
						return this.lexem;
					}
				}
				this.sb.Append((char)this.nextChar);
				this.nextChar = this.reader.Read();
			}
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

		public Color ConsumeColor()
		{
			int maxItems = 4;
			maxItems = this.ConsumeVector(maxItems);
#if WINDOWS_PHONE
			if (maxItems == 4)
			{
				return new Color(
					this.floatBuf[0], //r
					this.floatBuf[1], //g
					this.floatBuf[2], //b
					this.floatBuf[3]); //a
			}
			if (maxItems == 3)
			{
				return new Color(
					this.floatBuf[0], //r
					this.floatBuf[1], //g
					this.floatBuf[2] //b
					); 
			}
#else
			if (maxItems == 4)
			{
				return Color.FromArgb(
					ClampColor(this.floatBuf[0]),
					ClampColor(this.floatBuf[1]),
					ClampColor(this.floatBuf[2]),
					ClampColor(this.floatBuf[3]));
			}
			if (maxItems == 3)
			{
				return Color.FromArgb(255, ClampColor(this.floatBuf[0]), ClampColor(this.floatBuf[1]), ClampColor(this.floatBuf[2]));
			}
#endif
			throw new NotImplementedException();
		}

		public T ConsumeEnum<T>()
		{
			return (T)Enum.Parse(typeof(T), this.ConsumeString());
		}

		public float ConsumeFloat()
		{
			var f = float.Parse(this.Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public int ConsumeInt()
		{
			var f = int.Parse(this.Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public Quaternion ConsumeQuaternion()
		{
			int maxItems = 4;
			this.ConsumeVector(maxItems);
			return new Quaternion(this.floatBuf[1], this.floatBuf[2], this.floatBuf[3], this.floatBuf[0]);
		}

		public void ConsumeResourceReference(ResourceReference resourceReference)
		{
			this.ConsumeResourceReference(resourceReference, null);
		}

		public void ConsumeResourceReference(ResourceReference resourceReference, string folder)
		{
			var l = this.Lexem;
			this.Consume();
			if (l.IndexOfAny(new[] { '\\', '/' }, 0) >= 0)
			{
				resourceReference.FileReference = l;
				return;
			}
			if (File.Exists(Path.Combine(this.BasePath, l)))
			{
				resourceReference.FileReference = l;
				return;
			}
			if (folder != null)
			{
				var combinedPath = Path.Combine(folder, l);
				if (File.Exists(Path.Combine(this.BasePath, combinedPath)))
				{
					resourceReference.FileReference = combinedPath;
					return;
				}
			}
			resourceReference.NameReference = l;
		}

		public short ConsumeShort()
		{
			var f = short.Parse(this.Lexem, CultureInfo.InvariantCulture);
			this.Consume();
			return f;
		}

		public string ConsumeString()
		{
			var l = this.Lexem;
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

		public void Error(string message)
		{
			throw new TextParserException(message);
		}

		public void Skip(string s)
		{
			if (this.Lexem == s)
			{
				this.Consume();
			}
		}

		public void UnknownLexem()
		{
			var where = "input stream";
			var textReader = this.reader;
			if (textReader is StreamReader)
			{
				var baseStream = ((StreamReader)textReader).BaseStream;
				if (baseStream is FileStream)
				{
					where = ((FileStream)baseStream).Name;
				}
			}
			throw new TextParserException(
				string.Format(CultureInfo.InvariantCulture, "Unknown element \"{0}\" in {1}", this.lexem, where));
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
				var l = this.Lexem;
				if (l == "}")
				{
					this.Consume();
					return index;
				}
				this.floatBuf[index] = this.ConsumeFloat();
				if (this.Lexem == ",")
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
			retryToReadLexem:
			while (this.nextChar >= 0 && char.IsWhiteSpace((char)this.nextChar))
			{
				this.nextChar = this.reader.Read();
			}
			if (this.nextChar == '#')
			{
				while (this.nextChar >= 0 && this.nextChar != '\n' && this.nextChar != '\r')
				{
					this.nextChar = this.reader.Read();
				}
				goto retryToReadLexem;
			}
			if (this.nextChar == '/')
			{
				this.nextChar = this.reader.Read();
				if (this.nextChar != '/' && this.nextChar != '*')
				{
					this.lexem = "/";
					this.lexemReady = true;
					return;
				}

				if (this.nextChar == '*')
				{
					for (;;)
					{
						this.nextChar = this.reader.Read();
						if (this.nextChar == '*')
						{
							this.nextChar = this.reader.Read();
							if (this.nextChar == '/')
							{
								this.nextChar = this.reader.Read();
								break;
							}
						}
					}
				}
				else
				{
					while (this.nextChar >= 0 && this.nextChar != '\n' && this.nextChar != '\r')
					{
						this.nextChar = this.reader.Read();
					}
				}
				goto retryToReadLexem;
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
				case '-':
				case '+':
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
			if (this.nextChar >= 0 && (this.nextChar == '-' || this.nextChar == '+'))
			{
				this.sb.Append((char)this.nextChar);
				this.nextChar = this.reader.Read();
			}
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
			if (this.nextChar == 'E' || this.nextChar == 'e')
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
				//if (this.nextChar == '\\')
				//{
				//    this.nextChar = this.reader.Read();
				//    switch (this.nextChar)
				//    {
				//        default:
				//            this.sb.Append((char)this.nextChar);
				//            break;
				//    }
				//}
				//else
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
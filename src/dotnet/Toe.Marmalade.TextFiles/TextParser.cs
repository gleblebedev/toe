using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

using OpenTK;

using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.TextParser;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Marmalade.TextFiles
{
	public class TextParser: BaseTextParser
	{
		#region Constants and Fields

		private readonly string basePath;

		private readonly float[] floatBuf = new float[4];

		private readonly TextReader reader;

		private readonly IResourceFile resourceFile;

		private readonly StringBuilder sb = new StringBuilder(32);

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

		public IResourceFile ResourceFile
		{
			get
			{
				return this.resourceFile;
			}
		}

		#endregion

		#region Public Methods and Operators




		public string ConsumeBlock()
		{
			this.Consume("{");
			int depth = 1;
			this.sb.Clear();
			if (this.IsLexemReady)
			{
				this.sb.Append(this.Lexem);
			}
			for (;;)
			{
				if (this.nextChar < 0)
				{
					string res = this.sb.ToString();
					this.Lexem = res;
					this.Consume();
					return res;
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
						this.ReadNextChar();

						string res = this.sb.ToString();
						this.Lexem = res;
						this.Consume();
						return res;
					}
				}
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
			}
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


		public Quaternion ConsumeQuaternion()
		{
			this.ConsumeVector(4);
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

		

		public string ConsumeString()
		{
			var l = this.Lexem;
			//TODO: check if it is string
			this.Consume();
			return l;
		}

		public Vector2 ConsumeVector2()
		{
			this.ConsumeVector(2);
			return new Vector2(this.floatBuf[0], this.floatBuf[1]);
		}

		public Vector3 ConsumeVector3()
		{
			this.ConsumeVector(3);
			return new Vector3(this.floatBuf[0], this.floatBuf[1], this.floatBuf[2]);
		}

	
		#endregion

		#region Methods

		private static int ClampColor(float f)
		{
			return Math.Max(0, Math.Min(255, (int)(f * 255)));
		}
		protected override string GetSourceName()
		{
			var where = "input stream";
			var textReader = this.reader;
			var streamReader = textReader as StreamReader;
			if (streamReader != null)
			{
				var baseStream = (streamReader).BaseStream;
				var fileStream = baseStream as FileStream;
				if (fileStream != null)
				{
					@where = (fileStream).Name;
				}
			}
			return @where;
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
			this.Lexem = val;
			this.ReadNextChar();
		}

		private void ReadId()
		{
			this.sb.Clear();
			while (this.nextChar >= 0 && !char.IsWhiteSpace((char)this.nextChar) && this.nextChar != '{' && this.nextChar != '}'
			       && this.nextChar != '(' && this.nextChar != ')' && this.nextChar != '=' && this.nextChar != ','
			       && this.nextChar != ';')
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
			}
			this.Lexem = this.sb.ToString();
		}

		protected override void ReadLexem()
		{
			retryToReadLexem:
			while (this.nextChar >= 0 && char.IsWhiteSpace((char)this.nextChar))
			{
				this.ReadNextChar();
			}
			if (this.nextChar == '#')
			{
				while (this.nextChar >= 0 && this.nextChar != '\n' && this.nextChar != '\r')
				{
					this.ReadNextChar();
				}
				goto retryToReadLexem;
			}
			if (this.nextChar == '/')
			{
				this.ReadNextChar();
				if (this.nextChar != '/' && this.nextChar != '*')
				{
					this.Lexem = "/";
					return;
				}

				if (this.nextChar == '*')
				{
					for (;;)
					{
						this.ReadNextChar();
						if (this.nextChar == '*')
						{
							this.ReadNextChar();
							if (this.nextChar == '/')
							{
								this.ReadNextChar();
								break;
							}
						}
					}
				}
				else
				{
					while (this.nextChar >= 0 && this.nextChar != '\n' && this.nextChar != '\r')
					{
						this.ReadNextChar();
					}
				}
				goto retryToReadLexem;
			}
			switch (this.nextChar)
			{
				case -1:
					this.Lexem = null;
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

		private int ReadNextChar()
		{
			return this.nextChar = this.reader.Read();
		}

		private void ReadNumber()
		{
			this.sb.Clear();
			if (this.nextChar >= 0 && (this.nextChar == '-' || this.nextChar == '+'))
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
			}
			while (this.nextChar >= 0 && this.nextChar >= '0' && this.nextChar <= '9')
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
			}
			if (this.nextChar == '.')
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
				while (this.nextChar >= 0 && this.nextChar >= '0' && this.nextChar <= '9')
				{
					this.sb.Append((char)this.nextChar);
					this.ReadNextChar();
				}
			}
			if (this.nextChar == 'E' || this.nextChar == 'e')
			{
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
				if (this.nextChar == '-' || this.nextChar == '+')
				{
					this.sb.Append((char)this.nextChar);
					this.ReadNextChar();
				}
				while (this.nextChar >= 0 && this.nextChar >= '0' && this.nextChar <= '9')
				{
					this.sb.Append((char)this.nextChar);
					this.ReadNextChar();
				}
			}
			this.Lexem = this.sb.ToString();
		}

		private void ReadString()
		{
			var term = this.nextChar;
			this.ReadNextChar();
			this.sb.Clear();
			while (this.nextChar >= 0 && this.nextChar != term)
			{
				//if (this.nextChar == '\\')
				//{
				//    this.ReadNextChar();
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
				this.ReadNextChar();
			}
			this.OnTerminalSymbol(this.sb.ToString());
		}

		#endregion
	}
}
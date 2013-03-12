using System.IO;
using System.Text;

using Toe.Utils.TextParser;

namespace Toe.Utils.Mesh.Svg
{
	public class PathParser:BaseTextParser
	{
		private readonly TextReader reader;

		private int nextChar;

		private readonly StringBuilder sb = new StringBuilder();

		#region Constructors and Destructors

		public PathParser(TextReader reader)
		{
			this.reader = reader;
			this.nextChar = reader.Read();
		}

		#endregion

		protected int ReadNextChar()
		{
			this.nextChar = this.reader.Read();
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

			if (this.nextChar == ',')
			{
				this.Lexem = ",";
				this.ReadNextChar();
				return;
			}
			if (char.IsDigit((char)this.nextChar) || this.nextChar == '-' || this.nextChar == '+')
			{
				sb.Clear();
				sb.Append((char)this.nextChar);
				this.ReadNextChar();
				while (char.IsDigit((char)this.nextChar))
				{
					sb.Append((char)this.nextChar);
					this.ReadNextChar();
				}
				if (this.nextChar == '.')
				{
					sb.Append((char)this.nextChar);
					this.ReadNextChar();
					while (char.IsDigit((char)this.nextChar))
					{
						sb.Append((char)this.nextChar);
						this.ReadNextChar();
					}
				}
				this.Lexem = sb.ToString();
				return;
			}
			this.Lexem = new string(new []{ char.ToLower((char)this.nextChar)});
			this.ReadNextChar();
			return;
		}
	}
}
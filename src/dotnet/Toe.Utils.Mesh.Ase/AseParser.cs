using System.IO;
using System.Text;

using Toe.Utils.TextParser;

namespace Toe.Utils.Mesh.Ase
{
	public class AseParser : BaseTextParser
	{
		private TextReader reader;

		private int nextChar;

		readonly StringBuilder sb = new StringBuilder();

		private int line = 0;

		private int pos = 0;

		public AseParser(TextReader reader)
		{
			this.reader = reader;
			this.nextChar = reader.Read();
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
			return string.Format("{0} line:{1} pos:{2}", @where, this.line, this.pos);
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
	}
}
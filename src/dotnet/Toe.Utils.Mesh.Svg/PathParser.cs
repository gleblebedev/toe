using System.IO;
using System.Text;

using OpenTK;

using Toe.Utils.TextParser;

namespace Toe.Utils.Mesh.Svg
{
	public class PathParser : BaseTextParser
	{
		#region Constants and Fields

		private readonly TextReader reader;

		private readonly StringBuilder sb = new StringBuilder();

		private int nextChar;

		#endregion

		#region Constructors and Destructors

		public PathParser(TextReader reader)
		{
			this.reader = reader;
			this.nextChar = reader.Read();
		}

		#endregion

		#region Public Methods and Operators

		public new float ConsumeFloat()
		{
			var v = base.ConsumeFloat();
			this.Skip(",");
			return v;
		}

		public Vector2 ConsumeVector()
		{
			var x = this.ConsumeFloat();
			this.Skip(",");
			var y = this.ConsumeFloat();
			this.Skip(",");
			return new Vector2(x, y);
		}

		#endregion

		#region Methods

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
				this.sb.Clear();
				this.sb.Append((char)this.nextChar);
				this.ReadNextChar();
				while (char.IsDigit((char)this.nextChar))
				{
					this.sb.Append((char)this.nextChar);
					this.ReadNextChar();
				}
				if (this.nextChar == '.')
				{
					this.sb.Append((char)this.nextChar);
					this.ReadNextChar();
					while (char.IsDigit((char)this.nextChar))
					{
						this.sb.Append((char)this.nextChar);
						this.ReadNextChar();
					}
				}
				this.Lexem = this.sb.ToString();
				return;
			}

			this.sb.Clear();
			this.sb.Append((char)this.nextChar);
			this.ReadNextChar();
			//while (this.nextChar > 0 && char.IsLetter((char)this.nextChar))
			//{
			//    sb.Append((char)this.nextChar);
			//    this.ReadNextChar();
			//}
			this.Lexem = this.sb.ToString();
		}

		protected int ReadNextChar()
		{
			this.nextChar = this.reader.Read();
			return this.nextChar;
		}

		#endregion
	}
}
using System.Globalization;
using System.IO;
using System.Linq;

using Toe.Resources;
using Toe.Utils.TextParser;

namespace Toe.Marmalade.TextFiles
{
	public class TextSerializer : BaseTextSerializer
	{
		private TextWriter reader;

		private string basePath;

		private string depth = "";

		private bool isNewLine = true;

		public TextSerializer(TextWriter reader, string basePath)
		{
			this.reader = reader;
			this.basePath = basePath;
		}

		public void WriteAttribute(string className)
		{
			StartNewLine();
			reader.Write(className);
			isNewLine = false;
		}

		private void StartNewLine()
		{
			if (isNewLine)
				return;
			reader.WriteLine();
			reader.Write(depth);
			isNewLine = false;
		}

		public void OpenBlock()
		{
			StartNewLine();
			reader.Write("{");
			depth += "\t";
			isNewLine = false;
		}

		public void CloseBlock()
		{
			depth = depth.Substring(0,depth.Length-1);
			StartNewLine();
			reader.Write("}");
			isNewLine = false;
		}

		public void WriteStringValue(string name)
		{
			reader.Write(" ");
			reader.Write("\"");
			if (!string.IsNullOrEmpty(name))
				reader.Write(name.Replace("\"","\\\""));
			reader.Write("\"");
			isNewLine = false;
		}
		public void WriteStringValue(string name,string defaultValue)
		{
			reader.Write(" ");
			reader.Write("\"");
			if (name!=null)
				reader.Write(name.Replace("\"", "\\\""));
			else
			{
				reader.Write(defaultValue.Replace("\"", "\\\""));
			}
			reader.Write("\"");
			isNewLine = false;
		}
		public void WriteFloatValue(float i)
		{
			reader.Write(" ");
			reader.Write(string.Format(CultureInfo.InvariantCulture,"{0}",i));
		}

		public void WriteIntValue(int i)
		{
			reader.Write(" ");
			reader.Write(string.Format(CultureInfo.InvariantCulture, "{0}", i));
		}

		public void WriteRaw(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				reader.Write(str);
				isNewLine = (str.Last() == '\n' || str.Last() == '\r');
			}
		}
	}
}
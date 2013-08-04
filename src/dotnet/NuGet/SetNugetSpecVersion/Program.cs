using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SetNugetSpecVersion
{
	class Program
	{
		static private string versionText = "1.0.0.0";

		static void Main(string[] args)
		{
			if (args.Length > 0) versionText = args[0];
			foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(),"Toe.*.nuspec", SearchOption.AllDirectories))
			{
				if (Path.GetFileName(Path.GetDirectoryName(file)) == "nuget")
				{
					FixFile(file);
				}
			}
			foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(), "AssemblyInfo.cs", SearchOption.AllDirectories))
			{
				if (Path.GetFileName(Path.GetDirectoryName(file)) == "Properties")
				{
					FixAssemblyInfo(file);
				}
			}
		}

		private static void FixAssemblyInfo(string file)
		{
			var txt = File.ReadAllText(file);
			txt = ReplaceContent(txt, "[assembly: AssemblyVersion(\"", "\")]", versionText);
			txt = ReplaceContent(txt, "[assembly: AssemblyFileVersion(\"", "\")]", versionText);
			File.WriteAllText(file,txt,Encoding.UTF8);
		}

		private static string ReplaceContent(string txt, string start, string end, string val)
		{
			StringBuilder sb = new StringBuilder();

			
			int iterationStart = 0;
			for (; ; )
			{
				var i0 = txt.IndexOf(start, iterationStart, StringComparison.InvariantCulture);
				if (i0 < 0)
				{
					goto addTail;
				}
				;
				i0 += start.Length;
				if (i0+end.Length > txt.Length)
				{
					goto addTail;
				}

				var i1 = txt.IndexOf(end, i0, StringComparison.InvariantCulture);
				if (i1 < 0)
				{
					goto addTail;
				}
				;
				sb.Append(txt.Substring(iterationStart, i0 - iterationStart));
				sb.Append(val);
				sb.Append(end);
				iterationStart = i1 + end.Length;
			}
			addTail:
			if (iterationStart != txt.Length)
				sb.Append(txt.Substring(iterationStart, txt.Length - iterationStart));
			return sb.ToString();
		}

		private static void FixFile(string file)
		{
			var doc = XDocument.Load(file);
			var version = doc.Descendants(XName.Get("version", "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd")).First();
			version.Value = versionText;
			foreach (var dependency in doc.Descendants(XName.Get("dependency", "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd")))
			{
				var id = dependency.Attribute("id");
				if (id.Value.StartsWith("Toe."))
				{
					dependency.Attribute("version").SetValue(versionText);
				}
			}
			using (var s = XmlWriter.Create(File.Open(file,FileMode.OpenOrCreate,FileAccess.Write), new XmlWriterSettings{Indent = true}))
			{
				doc.WriteTo(s);
			}
		}
	}
}

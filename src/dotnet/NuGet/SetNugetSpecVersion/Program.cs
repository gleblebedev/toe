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
		static void Main(string[] args)
		{
			
			foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(),"Toe.*.nuspec", SearchOption.AllDirectories))
			{
				if (Path.GetFileName(Path.GetDirectoryName(file)) == "nuget")
				{
					FixFile(file);
				}
			}
		}

		private static void FixFile(string file)
		{
			var doc = XDocument.Load(file);
			var version = doc.Descendants(XName.Get("version", "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd")).First();
			version.Value = "1.0.0.4";
			using (var s = XmlWriter.Create(File.Open(file,FileMode.OpenOrCreate,FileAccess.Write), new XmlWriterSettings{Indent = true}))
			{
				doc.WriteTo(s);
			}
		}
	}
}

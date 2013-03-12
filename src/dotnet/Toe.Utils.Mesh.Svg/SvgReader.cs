using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgReader : ISceneReader
	{
		private string basePath;

		private SvgSchema schema;

		private XElement svg;

		#region Implementation of ISceneReader

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <param name="basePath">Base path to resources.</param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream, string basePath)
		{
						this.basePath = basePath ?? Directory.GetCurrentDirectory();
			var scene = new Scene();

			var doc = XDocument.Load(stream);
			var r = doc.Root;
			this.schema = new SvgSchema(r.Name.Namespace.NamespaceName);
			this.svg = doc.Element(this.schema.svgName);

			ParsePaths(scene, svg.Descendants(this.schema.pathName));

			return scene;
		}

		private void ParsePaths(Scene scene, IEnumerable<XElement> paths)
		{
			foreach (var path in paths)
			{
				this.ParsePath(scene,path);
			}
		}

		private void ParsePath(Scene scene, XElement path)
		{
			var d = path.Attribute(schema.dAttrName);
			if (d == null)
				return;
			var items = d.Value.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);


			//m 568.6259,522.8354 -27.832,-72.9004 10.3027,0 23.0957,61.377 23.1445,-61.377 10.2539,0 -27.7832,72.9004 L 568.6259,522.8354 Z
		}

		#endregion
	}

	public class SvgSchema
	{
		private readonly string namespaceName;

		public readonly XName svgName;

		public readonly XName pathName;

		public XName dAttrName;

		public SvgSchema(string namespaceName)
		{
			this.namespaceName = namespaceName;
			svgName = XName.Get("svg", this.namespaceName);
			pathName = XName.Get("path", this.namespaceName);
			dAttrName = XName.Get("d", "");
		}
	}
}
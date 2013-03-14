using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgReader : ISceneReader
	{
		private readonly SvgReaderOptions options;

		private string basePath;

		private SvgSchema schema;

		private XElement svg;

		public SvgReader(SvgReaderOptions options)
		{
			this.options = options;
		}

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
				this.ParsePath(path);
			}
		}

		private void ParsePath(XElement pathElement)
		{
			var path = new SvgPath();

			var d = pathElement.Attribute(schema.dAttrName);
			if (d == null)
				return;

			using (var stringReader = new StringReader(d.Value))
			{
				var p = new PathParser(stringReader);
				SvgPathCommand previousCommand = SvgPathCommand.MoveTo;
				bool absolute = true;
				for (; ; )
				{
					var verb = p.Lexem;
					if (verb == null)
						break;

					switch (verb)
					{
						case "M":
							previousCommand = SvgPathCommand.MoveTo;
							absolute = true;
							p.Consume();
							break;
						case "m":
							previousCommand = SvgPathCommand.MoveTo;
							absolute = false;
							p.Consume();
							break;
						case "Z":
						case "z":
							previousCommand = SvgPathCommand.ClosePath;
							absolute = true;
							p.Consume();
							break;
						case "L":
							previousCommand = SvgPathCommand.LineTo;
							absolute = true;
							p.Consume();
							break;
						case "l":
							previousCommand = SvgPathCommand.LineTo;
							absolute = false;
							p.Consume();
							break;
						case "H":
							previousCommand = SvgPathCommand.HorisontalLineTo;
							absolute = true;
							p.Consume();
							break;
						case "h":
							previousCommand = SvgPathCommand.HorisontalLineTo;
							absolute = false;
							p.Consume();
							break;
						case "V":
							previousCommand = SvgPathCommand.VerticalLineTo;
							absolute = true;
							p.Consume();
							break;
						case "v":
							previousCommand = SvgPathCommand.VerticalLineTo;
							absolute = false;
							p.Consume();
							break;
						case "C":
							previousCommand = SvgPathCommand.CubicBezier;
							absolute = true;
							p.Consume();
							break;
						case "c":
							previousCommand = SvgPathCommand.CubicBezier;
							absolute = false;
							p.Consume();
							break;
						case "S":
							previousCommand = SvgPathCommand.SmoothCubicBezier;
							absolute = true;
							p.Consume();
							break;
						case "s":
							previousCommand = SvgPathCommand.SmoothCubicBezier;
							absolute = false;
							p.Consume();
							break;
						case "Q":
							previousCommand = SvgPathCommand.QuadraticBezier;
							absolute = true;
							p.Consume();
							break;
						case "q":
							previousCommand = SvgPathCommand.QuadraticBezier;
							absolute = false;
							p.Consume();
							break;
						case "T":
							previousCommand = SvgPathCommand.SmoothQuadraticBezier;
							absolute = true;
							p.Consume();
							break;
						case "t":
							previousCommand = SvgPathCommand.SmoothQuadraticBezier;
							absolute = false;
							p.Consume();
							break;
						case "A":
							previousCommand = SvgPathCommand.EllipticalArc;
							absolute = true;
							p.Consume();
							break;
						case "a":
							previousCommand = SvgPathCommand.EllipticalArc;
							absolute = false;
							p.Consume();
							break;
						default:
							if (!char.IsDigit(verb[0]) && verb[0] != '-')
								throw new SvgReaderException(string.Format("Unknown command {0}", verb));
							break;
					}
					switch (previousCommand)
					{
						case SvgPathCommand.MoveTo:
							p.ConsumeVector(options);
							break;
						case SvgPathCommand.LineTo:
							p.ConsumeVector(options);
							break;
						case SvgPathCommand.HorisontalLineTo:
							p.ConsumeFloat(options);
							break;
						case SvgPathCommand.VerticalLineTo:
							p.ConsumeFloat(options);
							break;
						case SvgPathCommand.CubicBezier:
							p.ConsumeVector(options);
							p.ConsumeVector(options);
							p.ConsumeVector(options);
							break;
						case SvgPathCommand.QuadraticBezier:
							p.ConsumeVector(options);
							p.ConsumeVector(options);
							break;
						case SvgPathCommand.ClosePath:
							break;
						case SvgPathCommand.SmoothCubicBezier:
							p.ConsumeVector(options);
							p.ConsumeVector(options);
							break;
						case SvgPathCommand.SmoothQuadraticBezier:
							p.ConsumeVector(options);
							break;
						case SvgPathCommand.EllipticalArc:
							p.ConsumeVector(options);
							p.Skip(",");
							p.ConsumeFloat(options);
							p.Skip(",");
							var large_arc_flag = p.ConsumeInt();
							p.Skip(",");
							var sweep_flag = p.ConsumeInt();
							p.Skip(",");
							p.ConsumeVector(options);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					Trace.WriteLine(verb);
				}
			}
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
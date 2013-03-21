using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace Toe.Utils.Mesh.Svg
{
	public class SvgReader : ISceneReader
	{
		#region Constants and Fields

		private readonly ISceneReaderErrorHandler errorHandler;

		private readonly SvgReaderOptions options;

		private string basePath;

		private SvgSchema schema;

		private XElement svg;

		#endregion

		#region Constructors and Destructors

		public SvgReader(SvgReaderOptions options)
			: this(options, null)
		{
		}

		public SvgReader(SvgReaderOptions options, ISceneReaderErrorHandler errorHandler)
		{
			this.options = options;
			this.errorHandler = errorHandler ?? new SceneReaderErrorHandler();
		}

		#endregion

		#region Public Methods and Operators

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

			XDocument doc;
			var p = stream.Position;
			try
			{
				doc = XDocument.Load(stream);
			}
			catch (XmlException ex)
			{
				stream.Position = p;
				using (var s = new GZipStream(stream, CompressionMode.Decompress, true))
				{
					doc = XDocument.Load(s);
				}
			}

			var r = doc.Root;
			this.schema = new SvgSchema(r.Name.Namespace.NamespaceName);
			this.svg = doc.Element(this.schema.svgName);

			this.ParsePaths(scene, this.svg.Descendants(this.schema.pathName));

			return scene;
		}

		#endregion

		#region Methods

		private void ParsePath(XElement pathElement)
		{
			var path = new SvgPath();

			var d = pathElement.Attribute(this.schema.dAttrName);
			if (d == null)
			{
				return;
			}

			var value = d.Value;
			this.ReadPath(value);
			//m 568.6259,522.8354 -27.832,-72.9004 10.3027,0 23.0957,61.377 23.1445,-61.377 10.2539,0 -27.7832,72.9004 L 568.6259,522.8354 Z
		}

		private void ParsePaths(Scene scene, IEnumerable<XElement> paths)
		{
			foreach (var path in paths)
			{
				this.ParsePath(path);
			}
		}

		private SvgPath ReadPath(string value)
		{
			SvgPath path = new SvgPath();
			using (var stringReader = new StringReader(value))
			{
				var p = new PathParser(stringReader);
				SvgPathCommand previousCommand = SvgPathCommand.MoveTo;
				bool absolute = true;
				for (;;)
				{
					var verb = p.Lexem;
					if (verb == null)
					{
						break;
					}

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
							{
								this.errorHandler.Warning(string.Format("Unknown command {0}", verb));
								return path;
							}
							break;
					}
					switch (previousCommand)
					{
						case SvgPathCommand.MoveTo:
							p.ConsumeVector();
							break;
						case SvgPathCommand.LineTo:
							p.ConsumeVector();
							break;
						case SvgPathCommand.HorisontalLineTo:
							p.ConsumeFloat();
							break;
						case SvgPathCommand.VerticalLineTo:
							p.ConsumeFloat();
							break;
						case SvgPathCommand.CubicBezier:
							p.ConsumeVector();
							p.ConsumeVector();
							p.ConsumeVector();
							break;
						case SvgPathCommand.QuadraticBezier:
							p.ConsumeVector();
							p.ConsumeVector();
							break;
						case SvgPathCommand.ClosePath:
							break;
						case SvgPathCommand.SmoothCubicBezier:
							p.ConsumeVector();
							p.ConsumeVector();
							break;
						case SvgPathCommand.SmoothQuadraticBezier:
							p.ConsumeVector();
							break;
						case SvgPathCommand.EllipticalArc:
							p.ConsumeVector();
							p.Skip(",");
							p.ConsumeFloat();
							p.Skip(",");
							var large_arc_flag = p.ConsumeInt();
							p.Skip(",");
							var sweep_flag = p.ConsumeInt();
							p.Skip(",");
							p.ConsumeVector();
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					Trace.WriteLine(verb);
				}
			}
			return path;
		}

		#endregion
	}

	public class SvgSchema
	{
		#region Constants and Fields

		public readonly XName pathName;

		public readonly XName svgName;

		public XName dAttrName;

		private readonly string namespaceName;

		#endregion

		#region Constructors and Destructors

		public SvgSchema(string namespaceName)
		{
			this.namespaceName = namespaceName;
			this.svgName = XName.Get("svg", this.namespaceName);
			this.pathName = XName.Get("path", this.namespaceName);
			this.dAttrName = XName.Get("d", "");
		}

		#endregion
	}
}
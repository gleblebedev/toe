using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Autofac;

using Toe.Editors.Interfaces;

namespace Toe.Editors.Marmalade
{
	public class ResourceEditorFactory : IResourceEditorFactory
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IFileFormatInfo[] supportedFormats;

		#endregion

		#region Constructors and Destructors

		public ResourceEditorFactory(IComponentContext context)
		{
			this.supportedFormats = new IFileFormatInfo[]
				{
					new FileFormatInfo((a, b) => this.CreateEmptyFile(a, b, "CIwMaterial"))
						{
							CanCreate = true,
							DefaultFileName = "material.mtl",
							Extensions = new[] { ".mtl" },
							Factory = this,
							Name = "Marmalade SDK Material"
						},
					new FileFormatInfo((a, b) => this.CreateEmptyFile(a, b, "CIwModel"))
						{
							CanCreate = true,
							DefaultFileName = "geometry.geo",
							Extensions = new[] { ".geo" },
							Factory = this,
							Name = "Marmalade SDK Geometry"
						},
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".skin" }, Factory = this, Name = "Marmalade SDK Skin" },
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".skel" }, Factory = this, Name = "Marmalade SDK Skeleton" },
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".anim" }, Factory = this, Name = "Marmalade SDK Animation" },
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".itx" }, Factory = this, Name = "Marmalade SDK Generic ITX  File" },
					new FileFormatInfo
						{
							CanCreate = false,
							Extensions = new[] { ".bmp", ".png", ".jpg", ".tga" },
							Factory = this,
							Name = "Marmalade SDK Texture"
						},
					new FileFormatInfo((a, b) => this.CreateEmptyFile(a, b, "CIwResGroup"))
						{
							CanCreate = true,
							DefaultFileName = "group.group",
							Extensions = new[] { ".group" },
							Factory = this,
							Name = "Marmalade SDK Group"
						},
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".group.bin" }, Factory = this, Name = "Marmalade SDK Binary Group" },
				};
			this.context = context;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Name of resource editor group.
		/// </summary>
		public string Name
		{
			get
			{
				return "Marmalade SDK";
			}
		}

		/// <summary>
		/// All supported file formats.
		/// </summary>
		public IList<IFileFormatInfo> SupportedFormats
		{
			get
			{
				return this.supportedFormats;
			}
		}

		#endregion

		#region Public Methods and Operators

		public IResourceEditor CreateEditor(string fileName)
		{
			if (
				this.supportedFormats.SelectMany(supportedFormat => supportedFormat.Extensions).Any(
					extension => fileName.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)))
			{
				return this.context.Resolve<ResourceFileEditor>();
			}
			return null;
		}

		#endregion

		#region Methods

		private void CreateEmptyFile(string fileName, Stream stream, string className)
		{
			var name = Path.GetFileNameWithoutExtension(fileName);
			var buffer = Encoding.UTF8.GetBytes(string.Format("{0}\n{{\n\tname \"{1}\"\n}}\n", className, name));
			stream.Write(buffer, 0, buffer.Length);
		}

		#endregion
	}
}
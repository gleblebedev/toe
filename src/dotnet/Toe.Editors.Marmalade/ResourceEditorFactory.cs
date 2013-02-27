using System;
using System.Collections.Generic;
using System.Linq;

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
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".mtl" }, Factory = this, Name = "Marmalade SDK Material" },
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".geo" }, Factory = this, Name = "Marmalade SDK Geometry" },
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
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".group" }, Factory = this, Name = "Marmalade SDK Group" },
					new FileFormatInfo
						{ CanCreate = false, Extensions = new[] { ".group.bin" }, Factory = this, Name = "Marmalade SDK Binary Group" },
				};
			this.context = context;
		}

		#endregion

		#region Public Properties

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
	}
}
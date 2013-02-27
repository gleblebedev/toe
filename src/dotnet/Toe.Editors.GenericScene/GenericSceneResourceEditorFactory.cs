using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using Toe.Editors.Interfaces;

namespace Toe.Editors.GenericScene
{
	public class GenericSceneResourceEditorFactory : IResourceEditorFactory
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		private readonly IFileFormatInfo[] supportedFormats;

		#endregion

		#region Constructors and Destructors

		public GenericSceneResourceEditorFactory(IComponentContext context)
		{
			this.supportedFormats = new IFileFormatInfo[]
				{
					new FileFormatInfo{CanCreate = false,Extensions = new []{".ase"},Factory = this,Name = "ASCII file"},
					new FileFormatInfo{CanCreate = false,Extensions = new []{".bsp"},Factory = this,Name = "Quake/HalfLife BSP file"},
					new FileFormatInfo{CanCreate = false,Extensions = new []{".dae"},Factory = this,Name = "Collada DAE"},
				};
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

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

		public IResourceEditor CreateEditor(string fileName)
		{
			if (this.supportedFormats.SelectMany(supportedFormat => supportedFormat.Extensions).Any(extension => fileName.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)))
			{
				return this.context.Resolve<GenericSceneEditor>();
			}
			return null;
		}

		#endregion
	}
}
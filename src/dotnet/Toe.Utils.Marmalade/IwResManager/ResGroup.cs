using System;
using System.Collections.Generic;
using System.IO;

using Autofac;

using Toe.Resources;
using Toe.Utils.Marmalade;

namespace Toe.Utils.Mesh.Marmalade.IwResManager
{
	public class ResGroup : Managed
	{
		#region Constants and Fields

		public static readonly uint TypeHash = Hash.Get("CIwResGroup");

		private readonly IComponentContext context;

		private readonly IList<IResourceFile> externalResources;

		private readonly IResourceManager resourceManager;

		private bool isShared;

		#endregion

		#region Constructors and Destructors

		public ResGroup(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
			this.externalResources = this.context.Resolve<IList<IResourceFile>>();
		}

		#endregion

		#region Public Properties

		public IList<IResourceFile> ExternalResources
		{
			get
			{
				return this.externalResources;
			}
		}

		public bool IsShared
		{
			get
			{
				return isShared;
			}
			set
			{
				if (isShared != value)
				{
					this.RaisePropertyChanging("IsShared");
					isShared = value;
					this.RaisePropertyChanged("IsShared");
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public void AddFile(string fullPath)
		{
			var file = this.resourceManager.EnsureFile(fullPath);
			foreach (var f in this.externalResources)
			{
				if (f == file)
				{
					return;
				}
			}

			if (file != null)
			{
				this.externalResources.Add(file);

				var extension = Path.GetExtension(file.FilePath);
				if (string.Compare(extension, ".geo", StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					var mtl = Path.ChangeExtension(file.FilePath, ".mtl");
					if (File.Exists(mtl))
					{
						this.AddFile(mtl);
					}
				}
			}
		}

		public override uint ClassHashCode
		{
			get
			{
				return TypeHash;
			}
		}

		#endregion
	}
}
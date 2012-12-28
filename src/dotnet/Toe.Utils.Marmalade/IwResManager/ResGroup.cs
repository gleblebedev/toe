using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Autofac;

using Toe.Resources;

namespace Toe.Utils.Mesh.Marmalade.IwResManager
{
	public class ResGroup:Managed
	{
		private readonly IResourceManager resourceManager;

		private readonly IComponentContext context;

		public static readonly uint TypeHash = Hash.Get("CIwResGroup");

		#region Overrides of Managed

		public override uint GetClassHashCode()
		{
			return TypeHash;
		}

		#endregion

		public ResGroup(IResourceManager resourceManager, IComponentContext context)
		{
			this.resourceManager = resourceManager;
			this.context = context;
			this.externalResources = this.context.Resolve<IList<IResourceFile>>();
		}

		private readonly IList<IResourceFile> externalResources;

		public IList<IResourceFile> ExternalResources
		{
			get
			{
				return this.externalResources;
			}
		}

		public void AddFile(string fullPath)
		{
			var file = resourceManager.EnsureFile(fullPath);
			if(file != null)
				externalResources.Add(file);
		}
	}
}

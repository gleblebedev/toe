using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Toe.Utils.Mesh.Marmalade.IwResManager
{
	public class ResGroup:Managed
	{
		private readonly ObservableCollection<Managed> embeddedResources = new ObservableCollection<Managed>();
		private readonly ObservableCollection<ResourceFileReference> externalResources= new ObservableCollection<ResourceFileReference>();

		public ObservableCollection<Managed> EmbeddedResources
		{
			get
			{
				return this.embeddedResources;
			}
		}

		public ObservableCollection<ResourceFileReference> ExternalResources
		{
			get
			{
				return this.externalResources;
			}
		}
	}
}

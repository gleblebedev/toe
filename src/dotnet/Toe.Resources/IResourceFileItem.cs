using System.ComponentModel;

namespace Toe.Resources
{
	public interface IResourceFileItem: INotifyPropertyChanging, INotifyPropertyChanged
	{
		object Resource { get; }

		string Name { get; }
		uint NameHash { get; }

		uint Type { get; }
	}
}
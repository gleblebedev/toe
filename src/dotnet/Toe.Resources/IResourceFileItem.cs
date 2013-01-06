using System.ComponentModel;

namespace Toe.Resources
{
	public interface IResourceFileItem : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Public Properties

		string Name { get; }

		uint NameHash { get; }

		object Resource { get; }

		uint Type { get; }

		#endregion
	}
}
using System.Collections.Generic;

namespace Toe.Messaging
{
	public interface IMessageDescription
	{
		#region Public Properties

		int MinSize { get; }

		IEnumerable<PropertyDescription> Properties { get; }

		#endregion

		#region Public Methods and Operators

		void DefineProperty(string name, int propertyType, int offset, int size);

		PropertyDescription GetPropertyById(int propertyKey);

		#endregion
	}
}
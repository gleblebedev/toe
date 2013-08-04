using System;
using System.Collections.Generic;

namespace Toe.Messaging
{
	public interface IMessageDescription
	{
		#region Public Properties

		IEnumerable<PropertyDescription> Properties { get; }

		int MinSize { get; }

		#endregion

		#region Public Methods and Operators

		void DefineProperty(string name, int propertyType, int offset, int size);

		#endregion

		PropertyDescription GetPropertyById(int propertyKey);
	}
}
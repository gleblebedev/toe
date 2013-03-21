using System;

namespace Toe.Editors.Interfaces
{
	public interface IEditorConfigStorage
	{
		#region Public Methods and Operators

		object Load(Type t);

		void Save(object options);

		#endregion
	}
}
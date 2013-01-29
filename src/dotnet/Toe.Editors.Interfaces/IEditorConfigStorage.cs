using System;

namespace Toe.Editors.Interfaces
{
	public interface IEditorConfigStorage
	{
		object Load(Type t);
		void Save(object options);
	}
}
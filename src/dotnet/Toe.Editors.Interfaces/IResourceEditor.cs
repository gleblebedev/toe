using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces
{
	public interface IResourceEditorFactory
	{
		IResourceEditor CreateEditor(string filename);
	}
	public interface IResourceEditor : IDisposable
	{
		#region Public Properties

		Control Control { get; }

		string CurrentFileName { get;  }

		#endregion

		#region Public Methods and Operators

		void LoadFile(string filename);

		void RecordCommand(string command);

		void SaveFile(string filename);

		void StopRecorder();

		#endregion
	}
}
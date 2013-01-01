using System;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces
{
	public interface IResourceEditor : IDisposable
	{
		#region Public Properties

		bool CanCopy { get; }

		bool CanCut { get; }

		bool CanPaste { get; }

		bool CanRedo { get; }

		bool CanUndo { get; }

		Control Control { get; }

		string CurrentFileName { get; }

		#endregion

		#region Public Methods and Operators

		void Delete();

		void LoadFile(string filename);

		void RecordCommand(string command);

		void Redo();

		void SaveFile(string filename);

		void StopRecorder();

		void Undo();

		void onSelectAll();

		#endregion
	}
}
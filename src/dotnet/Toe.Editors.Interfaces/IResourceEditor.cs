using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Toe.Editors.Interfaces
{
	public interface IResourceEditor: IDisposable
	{
		void StopRecorder();
		Control Control { get; }
		void RecordCommand(string command);
		void SaveFile(string fileName);
		void LoadFile(string filename);
	}
}

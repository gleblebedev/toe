using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public partial class DefaultEditor : UserControl, IResourceEditor
	{
		public DefaultEditor()
		{
			InitializeComponent();
			this.Controls.Add(new Label(){Text = "File format is not supported"});
		}

		public void StopRecorder()
		{
			
		}

		Control IResourceEditor.Control
		{
			get { return this; }
		}

		public void RecordCommand(string command)
		{
			
		}

		public void SaveFile(string fileName)
		{
			
		}

		public void LoadFile(string filename)
		{
			
		}
	}
}

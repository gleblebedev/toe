using System.Windows.Forms;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public partial class DefaultEditor : UserControl, IResourceEditor
	{
		#region Constructors and Destructors

		public DefaultEditor()
		{
			this.InitializeComponent();
			this.Controls.Add(new Label { Text = "File format is not supported" });
		}

		#endregion

		#region Explicit Interface Properties

		Control IResourceEditor.Control
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void LoadFile(string filename)
		{
		}

		public void RecordCommand(string command)
		{
		}

		public void SaveFile(string fileName)
		{
		}

		public void StopRecorder()
		{
		}

		#endregion
	}
}
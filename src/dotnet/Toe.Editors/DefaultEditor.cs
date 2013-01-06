using System.Windows.Forms;

using Toe.Editors.Interfaces;

namespace Toe.Editors
{
	public partial class DefaultEditor : UserControl, IResourceEditor
	{
		#region Constants and Fields

		private readonly IEditorEnvironment editorEnvironment;

		private string currentFileName;

		#endregion

		#region Constructors and Destructors

		public DefaultEditor(IEditorEnvironment editorEnvironment)
		{
			this.editorEnvironment = editorEnvironment;
			this.InitializeComponent();
			this.Controls.Add(new Label { Text = "File format is not supported" });
		}

		#endregion

		#region Public Properties

		public string CurrentFileName
		{
			get
			{
				return this.currentFileName;
			}
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

		public void LoadFile(string fileName)
		{
			this.currentFileName = fileName;
		}

		public void RecordCommand(string command)
		{
		}

		public void SaveFile(string fileName)
		{
			this.currentFileName = fileName;
		}

		public void StopRecorder()
		{
		}

		#endregion
	}
}
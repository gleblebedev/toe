using System.Windows.Forms;

using Toe.Editors;
using Toe.Editors.Interfaces;

namespace Toe.Editor
{
	public partial class MainEditorWindow : Form
	{
		#region Constants and Fields

		private readonly EditorFactory factory;

		private readonly IResourceEditor resourceEditor;

		#endregion

		#region Constructors and Destructors

		public MainEditorWindow()
		{
			this.InitializeComponent();
			this.factory = new EditorFactory();
			this.resourceEditor = this.factory.CreateEditor(".geo");
			this.resourceEditor.Control.Dock = DockStyle.Fill;
			this.resourceEditor.LoadFile("male_legs_trousers0_lod0.geo");
			this.Controls.Add(this.resourceEditor.Control);
		}

		#endregion
	}
}
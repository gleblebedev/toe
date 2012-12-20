using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Toe.Editors;
using Toe.Editors.Interfaces;

namespace Toe.Editor
{
	public partial class MainEditorWindow : Form
	{
		private EditorFactory factory;
		private IResourceEditor resourceEditor;

		public MainEditorWindow()
		{
			InitializeComponent();
			factory = new EditorFactory();
			resourceEditor = factory.CreateEditor(".group");
			resourceEditor.Control.Dock = DockStyle.Fill;
			this.Controls.Add(resourceEditor.Control);
		}
	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using Toe.Editors.Interfaces;
using Toe.Editors.Interfaces.Bindings;
using Toe.Editors.Interfaces.Views;
using Toe.Marmalade;

namespace Toe.Editors.Marmalade.Views
{
	public class EditNameView : SingleControlView<TextBox>, IView
	{
		#region Constants and Fields

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private readonly ICommandHistory history;

		#endregion

		#region Constructors and Destructors

		public EditNameView(ICommandHistory history)
		{
			this.history = history;
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.dataContext.PropertyChanged += this.UpdateTextBoxIfNameChanged;
			this.ViewControl.TextChanged += this.UpdateDataContext;
		}

		private void UpdateTextBoxIfNameChanged(object sender, PropertyChangedEventArgs e)
		{
			this.UpdateTextBoxText();
		}

		#endregion

		#region Public Properties

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public Managed Managed
		{
			get
			{
				return this.dataContext.Value as Managed;
			}
		}

		#endregion

		#region Methods

		private void UpdateDataContext(object sender, EventArgs e)
		{
			if (this.ViewControl.ReadOnly)
			{
				return;
			}

			if (this.Managed.Name == this.ViewControl.Text)
			{
				return;
			}
			var setNameCommand = this.history.Top as SetNameCommand;
			if (setNameCommand != null)
			{
				if (Equals(setNameCommand.Managed, this.Managed))
				{
					setNameCommand.NewName = this.ViewControl.Text;
					setNameCommand.Redo();
					return;
				}
			}
			setNameCommand = new SetNameCommand(this.Managed, this.ViewControl.Text);
			setNameCommand.Redo();
			this.history.RegisterAction(setNameCommand);
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			this.UpdateTextBoxText();
		}

		private void UpdateTextBoxText()
		{
			Managed managed = this.Managed;
			if (managed == null)
			{
				this.ViewControl.ReadOnly = true;
				this.ViewControl.Text = string.Empty;
				return;
			}
			if (managed.NameHash == 0)
			{
				this.ViewControl.ReadOnly = false;
				this.ViewControl.Text = string.Empty;
				return;
			}
			if (string.IsNullOrEmpty(managed.Name))
			{
				this.ViewControl.ReadOnly = true;
				this.ViewControl.Text = string.Format("0x{0:X08}", managed.NameHash);
				return;
			}
			this.ViewControl.ReadOnly = false;
			this.ViewControl.Text = managed.Name;
		}

		#endregion

		public class SetNameCommand : ICommand
		{
			#region Constants and Fields

			private readonly Managed managed;

			private string newName;

			private readonly string oldName;

			private readonly uint oldNameHash;

			#endregion

			#region Constructors and Destructors

			public SetNameCommand(Managed managed, string newName)
			{
				this.managed = managed;
				this.newName = newName;
				this.oldName = managed.Name;
				this.oldNameHash = managed.NameHash;
			}

			public Managed Managed
			{
				get
				{
					return this.managed;
				}
			}

			public string NewName
			{
				get
				{
					return this.newName;
				}
				set
				{
					this.newName = value;
				}
			}

			public string OldName
			{
				get
				{
					return this.oldName;
				}
			}

			public uint OldNameHash
			{
				get
				{
					return this.oldNameHash;
				}
			}

			#endregion

			#region Public Methods and Operators

			public void Redo()
			{
				this.Managed.Name = this.NewName;
			}

			public void Undo()
			{
				if (!string.IsNullOrEmpty(this.OldName))
				{
					this.Managed.Name = this.OldName;
				}
				else
				{
					this.Managed.NameHash = this.OldNameHash;
				}
			}

			#endregion
		}
	}
}
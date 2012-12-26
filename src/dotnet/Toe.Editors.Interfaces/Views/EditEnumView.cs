using System;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditEnumView : UserControl, IView
	{
		readonly DataContextContainer dataContext = new DataContextContainer();

		private ComboBox comboBox;

		private EnumWellKnownValues wellKnownValues;
		public override System.Drawing.Size GetPreferredSize(System.Drawing.Size proposedSize)
		{
			return comboBox.GetPreferredSize(proposedSize);
		}
		public EditEnumView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.Height = 16;

			comboBox = new ComboBox();
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox.SelectedIndexChanged += UpdateDataContext;
			this.BindKnownValues();
			this.Controls.Add(comboBox);
		}

		private void UpdateDataContext(object sender, EventArgs e)
		{
			
		}

		private void BindKnownValues()
		{
			var v = comboBox.SelectedItem;
			this.comboBox.Items.Clear();
			if (wellKnownValues != null)
			{
				foreach (var enumWellKnownValue in wellKnownValues)
				{
					this.comboBox.Items.Add(enumWellKnownValue.Key);
				}
			}
			comboBox.SelectedItem = v;
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			if (this.dataContext.Value != null)
			{
				comboBox.SelectedItem = this.dataContext.Value;
			}
		}


		#region Implementation of IView

		public DataContextContainer DataContext
		{
			get
			{
				return this.dataContext;
			}
		}

		public EnumWellKnownValues WellKnownValues
		{
			get
			{
				return this.wellKnownValues;
			}
			set
			{
				if (this.wellKnownValues != value)
				{
					this.wellKnownValues = value;
					this.BindKnownValues();
				}
			}
		}

		#endregion
	}
}
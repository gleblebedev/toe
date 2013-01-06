using System;
using System.Drawing;
using System.Windows.Forms;

using Toe.Editors.Interfaces.Bindings;

namespace Toe.Editors.Interfaces.Views
{
	public class EditEnumView : UserControl, IView
	{
		#region Constants and Fields

		private readonly ComboBox comboBox;

		private readonly DataContextContainer dataContext = new DataContextContainer();

		private bool suppressValueChangeEvent;

		private EnumWellKnownValues wellKnownValues;

		#endregion

		#region Constructors and Destructors

		public EditEnumView()
		{
			this.dataContext.DataContextChanged += this.UpdateTextBox;
			this.Height = 16;

			this.comboBox = new ComboBox();
			this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox.SelectedIndexChanged += this.UpdateDataContext;
			this.BindKnownValues();
			this.Controls.Add(this.comboBox);
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

		#region Public Methods and Operators

		public override Size GetPreferredSize(Size proposedSize)
		{
			return this.comboBox.GetPreferredSize(proposedSize);
		}

		#endregion

		#region Methods

		private void BindKnownValues()
		{
			var v = this.comboBox.SelectedItem;

			this.suppressValueChangeEvent = true;
			try
			{
				this.comboBox.Items.Clear();
				if (this.wellKnownValues != null)
				{
					foreach (var enumWellKnownValue in this.wellKnownValues)
					{
						this.comboBox.Items.Add(enumWellKnownValue.Key);
					}
				}
				this.comboBox.SelectedItem = v;
			}
			finally
			{
				this.suppressValueChangeEvent = false;
			}
		}

		private void UpdateDataContext(object sender, EventArgs e)
		{
			if (this.suppressValueChangeEvent)
			{
				return;
			}
			this.suppressValueChangeEvent = true;
			try
			{
				var item = this.comboBox.SelectedItem;
				this.dataContext.Value = item;
			}
			finally
			{
				this.suppressValueChangeEvent = false;
			}
		}

		private void UpdateTextBox(object sender, DataContextChangedEventArgs e)
		{
			if (this.suppressValueChangeEvent)
			{
				return;
			}
			if (this.dataContext.Value != null)
			{
				this.suppressValueChangeEvent = true;
				try
				{
					this.comboBox.SelectedItem = this.dataContext.Value;
				}
				finally
				{
					this.suppressValueChangeEvent = false;
				}
			}
		}

		#endregion
	}
}
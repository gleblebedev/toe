using System.ComponentModel;

namespace Toe.Editors
{
	public class Base3DEditorOptions : INotifyPropertyChanged
	{
		#region Constants and Fields

		protected static PropertyChangedEventArgs LightingChanged = new PropertyChangedEventArgs("Lighting");

		protected static PropertyChangedEventArgs NormalsChanged = new PropertyChangedEventArgs("Normals");

		protected static PropertyChangedEventArgs WireframeChanged = new PropertyChangedEventArgs("Wireframe");

		private bool lighting = true;

		private bool normals;

		private bool wireframe;

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

		public bool Lighting
		{
			get
			{
				return this.lighting;
			}
			set
			{
				if (this.lighting != value)
				{
					this.lighting = value;
					this.RaisePropertyChanged(LightingChanged);
				}
			}
		}

		public bool Normals
		{
			get
			{
				return this.normals;
			}
			set
			{
				if (this.normals != value)
				{
					this.normals = value;
					this.RaisePropertyChanged(NormalsChanged);
				}
			}
		}

		public bool Wireframe
		{
			get
			{
				return this.wireframe;
			}
			set
			{
				if (this.wireframe != value)
				{
					this.wireframe = value;
					this.RaisePropertyChanged(WireframeChanged);
				}
			}
		}

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(PropertyChangedEventArgs property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, property);
			}
		}

		#endregion
	}
}
using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material.
	/// </summary>
	public class SceneItem : ISceneItem, INotifyPropertyChanged
	{
		#region Constants and Fields

		protected static PropertyChangedEventArgs ParametersChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<SceneItem>(a => a.Parameters));
		protected static PropertyChangedEventArgs IdChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<SceneItem>(a => a.Id));
		protected static PropertyChangedEventArgs NameChangedEventArgs = new PropertyChangedEventArgs(Expr.Path<SceneItem>(a => a.Name));

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		/// <summary>
		/// Identifier of the scene item.
		/// </summary>
		private string id;

		/// <summary>
		/// Name of the scene item.
		/// </summary>
		private string name;

		#endregion

		#region Public Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public Properties

		/// <summary>
		/// Identifier of the scene item.
		/// </summary>
		public string Id
		{
			get
			{
				return id;
			}
			set
			{
				if (id != value)
				{
					id = value;
					this.RaisePropertyChanged(IdChangedEventArgs);
				}
			}
		}

		/// <summary>
		/// Name of the scene item.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name != value)
				{
					name = value;
					this.RaisePropertyChanged(NameChangedEventArgs);
				}
			}
		}

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		public IParameterCollection Parameters
		{
			get
			{
				return this.parameters ?? (this.Parameters = new DynamicCollection());
			}
			set
			{
				if (this.parameters != value)
				{
					this.parameters = value;
					this.RaisePropertyChanged(ParametersChangedEventArgs);
				}
			}
		}

		#endregion

		#region Methods

		protected virtual void RaisePropertyChanged(string property)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}

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
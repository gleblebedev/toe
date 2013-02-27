using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	/// <summary>
	/// Generic material.
	/// </summary>
	public class SceneItem : ClassWithNotification, ISceneItem
	{
		#region Constants and Fields

		protected static PropertyChangedEventArgs IdChangedEventArgs =
			new PropertyChangedEventArgs(Expr.Path<SceneItem>(a => a.Id));

		protected static PropertyChangedEventArgs NameChangedEventArgs =
			new PropertyChangedEventArgs(Expr.Path<SceneItem>(a => a.Name));

		protected static PropertyChangedEventArgs ParametersChangedEventArgs =
			new PropertyChangedEventArgs(Expr.Path<SceneItem>(a => a.Parameters));

		protected static PropertyEventArgs RenderDataEventArgs = Expr.PropertyEventArgs<SceneItem>(x => x.RenderData);

		/// <summary>
		/// Identifier of the scene item.
		/// </summary>
		private string id;

		/// <summary>
		/// Name of the scene item.
		/// </summary>
		private string name;

		/// <summary>
		/// Collection of source specific parameters.
		/// </summary>
		private IParameterCollection parameters;

		private object renderData;

		#endregion

		#region Public Properties

		/// <summary>
		/// Identifier of the scene item.
		/// </summary>
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				if (this.id != value)
				{
					this.id = value;
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
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
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

		public object RenderData
		{
			get
			{
				return this.renderData;
			}
			set
			{
				if (this.renderData != value)
				{
					this.RaisePropertyChanging(RenderDataEventArgs.Changing);
					this.renderData = value;
					this.RaisePropertyChanged(RenderDataEventArgs.Changed);
				}
			}
		}

		#endregion
	}
}
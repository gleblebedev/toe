using System.Drawing;

namespace Toe.Utils.Mesh
{
	public enum CullMode
	{
		Back,

		Front,

		None,
	}

	public class SceneEffect : SceneItem, IEffect
	{
		#region Constants and Fields

		protected static PropertyEventArgs AmbientEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Ambient);

		protected static PropertyEventArgs CullModeEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.CullMode);

		protected static PropertyEventArgs DiffuseEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Diffuse);

		protected static PropertyEventArgs EmissionEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Emission);

		protected static PropertyEventArgs ReflectiveEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Reflective);

		protected static PropertyEventArgs ReflectivityEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Reflectivity);

		protected static PropertyEventArgs ShininessEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Shininess);

		protected static PropertyEventArgs SpecularEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Specular);

		protected static PropertyEventArgs TransparencyEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Transparency);

		protected static PropertyEventArgs TransparentEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Transparent);

		private IColorSource ambient;

		private CullMode cullMode = CullMode.Back;

		private IColorSource diffuse;

		private IColorSource emission;

		private IColorSource reflective;

		private float reflectivity;

		private float shininess;

		private IColorSource specular;

		private float transparency;

		private IColorSource transparent;

		#endregion

		#region Public Properties

		public IColorSource Ambient
		{
			get
			{
				if (this.ambient == null)
				{
					this.ambient = new SolidColorSource { Color = Color.Black };
				}
				return this.ambient;
			}
			set
			{
				if (this.ambient != value)
				{
					this.RaisePropertyChanging(AmbientEventArgs.Changing);
					this.ambient = value;
					this.RaisePropertyChanged(AmbientEventArgs.Changed);
				}
			}
		}

		public CullMode CullMode
		{
			get
			{
				return this.cullMode;
			}
			set
			{
				if (this.cullMode != value)
				{
					this.RaisePropertyChanging(CullModeEventArgs.Changing);
					this.cullMode = value;
					this.RaisePropertyChanged(CullModeEventArgs.Changed);
				}
			}
		}

		public IColorSource Diffuse
		{
			get
			{
				if (this.diffuse == null)
				{
					this.diffuse = new SolidColorSource { Color = Color.White };
				}
				return this.diffuse;
			}
			set
			{
				if (this.diffuse != value)
				{
					this.RaisePropertyChanging(DiffuseEventArgs.Changing);
					this.diffuse = value;
					this.RaisePropertyChanged(DiffuseEventArgs.Changed);
				}
			}
		}

		public IColorSource Emission
		{
			get
			{
				if (this.emission == null)
				{
					this.emission = new SolidColorSource { Color = Color.Black };
				}
				return this.emission;
			}
			set
			{
				if (this.emission != value)
				{
					this.RaisePropertyChanging(EmissionEventArgs.Changing);
					this.emission = value;
					this.RaisePropertyChanged(EmissionEventArgs.Changed);
				}
			}
		}

		public IColorSource Reflective
		{
			get
			{
				if (this.reflective == null)
				{
					this.reflective = new SolidColorSource { Color = Color.Black };
				}
				return this.reflective;
			}
			set
			{
				if (this.reflective != value)
				{
					this.RaisePropertyChanging(ReflectiveEventArgs.Changing);
					this.reflective = value;
					this.RaisePropertyChanged(ReflectiveEventArgs.Changed);
				}
			}
		}

		public float Reflectivity
		{
			get
			{
				return this.reflectivity;
			}
			set
			{
				if (this.reflectivity != value)
				{
					this.RaisePropertyChanging(ReflectivityEventArgs.Changing);
					this.reflectivity = value;
					this.RaisePropertyChanged(ReflectivityEventArgs.Changed);
				}
			}
		}

		public float Shininess
		{
			get
			{
				return this.shininess;
			}
			set
			{
				if (this.shininess != value)
				{
					this.RaisePropertyChanging(ShininessEventArgs.Changing);
					this.shininess = value;
					this.RaisePropertyChanged(ShininessEventArgs.Changed);
				}
			}
		}

		public IColorSource Specular
		{
			get
			{
				if (this.specular == null)
				{
					this.specular = new SolidColorSource { Color = Color.Black };
				}
				return this.specular;
			}
			set
			{
				if (this.specular != value)
				{
					this.RaisePropertyChanging(SpecularEventArgs.Changing);
					this.specular = value;
					this.RaisePropertyChanged(SpecularEventArgs.Changed);
				}
			}
		}

		public float Transparency
		{
			get
			{
				return this.transparency;
			}
			set
			{
				if (this.transparency != value)
				{
					this.RaisePropertyChanging(TransparencyEventArgs.Changing);
					this.transparency = value;
					this.RaisePropertyChanged(TransparencyEventArgs.Changed);
				}
			}
		}

		public IColorSource Transparent
		{
			get
			{
				return this.transparent;
			}
			set
			{
				if (this.transparent != value)
				{
					this.RaisePropertyChanging(TransparentEventArgs.Changing);
					this.transparent = value;
					this.RaisePropertyChanged(TransparentEventArgs.Changed);
				}
			}
		}

		#endregion
	}
}
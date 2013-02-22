using System.ComponentModel;

namespace Toe.Utils.Mesh
{
	public enum CullMode
	{
		Front,
		Back,
		None,
	}
	public class SceneEffect:SceneItem, IEffect
	{
		protected static PropertyEventArgs CullModeEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.CullMode);

		private CullMode cullMode;

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

		protected static PropertyEventArgs DiffuseEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Diffuse);

		private IColorSource diffuse;

		public IColorSource Diffuse
		{
			get
			{
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


		protected static PropertyEventArgs AmbientEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Ambient);

		private IColorSource ambient;

		public IColorSource Ambient
		{
			get
			{
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

		protected static PropertyEventArgs EmissionEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Emission);

		private IColorSource emission;

		public IColorSource Emission
		{
			get
			{
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


		protected static PropertyEventArgs SpecularEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Specular);

		private IColorSource specular;

		public IColorSource Specular
		{
			get
			{
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


		protected static PropertyEventArgs ReflectiveEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Reflective);

		private IColorSource reflective;

		public IColorSource Reflective
		{
			get
			{
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


		protected static PropertyEventArgs TransparentEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Transparent);

		private IColorSource transparent;

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



		protected static PropertyEventArgs ShininessEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Shininess);

		private float shininess;

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



		protected static PropertyEventArgs ReflectivityEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Reflectivity);

		private float reflectivity;

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


		protected static PropertyEventArgs TransparencyEventArgs = Expr.PropertyEventArgs<SceneEffect>(x => x.Transparency);

		private float transparency;

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
	}
}
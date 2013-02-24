using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

using OpenTK;

namespace Toe.Utils.Mesh.Bsp
{
	public abstract class BaseBspReader
	{
		#region Constants and Fields

		private IScene scene;

		private long startOfTheFile;

		private Stream stream;

		#endregion

		#region Public Properties

		public virtual string GameRootPath { get; set; }

		public IScene Scene
		{
			get
			{
				return this.scene;
			}
		}

		public long StartOfTheFile
		{
			get
			{
				return this.startOfTheFile;
			}
		}

		public Stream Stream
		{
			get
			{
				return this.stream;
			}
			set
			{
				this.stream = value;
				this.startOfTheFile = this.stream.Position;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Load generic scene from BSP file.
		/// </summary>
		/// <param name="stream"> </param>
		/// <returns>Loaded scene.</returns>
		public virtual IScene LoadScene()
		{
			this.scene = new Scene();

			this.ReadHeader();
			this.ReadModels();
			this.ReadVertices();
			this.ReadEdges();
			this.ReadPlanes();
			this.ReadTextures();
			this.ReadEffects();
			this.ReadLightmaps();
			this.ReadFaces();
			this.ReadNodes();
			this.ReadVisibilityList();
			this.ReadLeaves();
			this.ReadEntities();
			this.BuildScene();

			return this.scene;
		}

		#endregion

		#region Methods

		protected void AssertStreamPossition(long position)
		{
			if (this.Stream.Position != position)
			{
				throw new BspFormatException(
					string.Format("Unknow data format (file position {0}, expected {1})", this.Stream.Position, position));
			}
		}

		protected virtual void BuildEntityNodes(string entitiesInfo)
		{
			if (string.IsNullOrEmpty(entitiesInfo))
			{
				return;
			}
			var tp = new BaseEntityTextParser(new StringReader(entitiesInfo), this.ConvertEntityProperty);
			var enitities = tp.Skip(1).ToArray();

			foreach (dynamic enitity in enitities)
			{
				var origin = enitity.origin as Vector3?;
				if (origin.HasValue)
				{
					var node = new Node { Position = origin.Value };
					this.scene.Nodes.Add(node);
					var model = enitity.model as string;
					if (!string.IsNullOrEmpty(model))
					{
						if (model.StartsWith("*"))
						{
							int geomId = int.Parse(model.Substring(1), CultureInfo.InvariantCulture);
							node.Mesh = this.Scene.Geometries[geomId];
						}
					}
				}
			}
		}

		protected virtual void BuildScene()
		{
		}

		//Dictionary<string,string> knownProps = new Dictionary<string, string>();
		protected virtual object ConvertEntityProperty(string key, string val)
		{
			switch (key)
			{
				case "world_maxs": //1424 3288 848
				case "world_mins": //-2284 -2504 -272
				case "origin": //117.601 -507 57.7613
				case "lowerright": //142 867 -108
				case "lowerleft": //181 867 -108
				case "upperright": //142 867 -44
				case "upperleft": //181 867 -44
					return this.ParseVec3EntityProperty(val);

				case "overlaycolor": //0 0 0
				case "color": //120 133 143
				case "fogcolor2": //194 205 218
				case "fogcolor": //139 152 160
					return this.ParseColorEntityProperty(val);

				case "MaxRange": //1424 3288 848
					return float.Parse(val, CultureInfo.InvariantCulture);
				case "mapversion": //2855
					return int.Parse(val, CultureInfo.InvariantCulture);
					/*
				case "spawnflags": //0
					return val;

				case "skyname": //italy
					return val;

				case "maxpropscreenwidth": //-1
					return val;

				case "detailvbsp": //detail.vbsp
					return val;

				case "detailmaterial": //detail/detailsprites
					return val;

				case "classname": //worldspawn
					return val;

				case "hammerid": //0
					return val;


				case "Width": //1.4
					return val;

				case "Type": //0
					return val;

				case "TextureScale": //1
					return val;

				case "Subdiv": //2
					return val;

				case "Slack": //125
					return val;

				case "RopeMaterial": //cable/cable.vmt
					return val;

				case "PositionInterpolator": //2
					return val;

				case "NextKey": //elec_wire_08
					return val;

				case "MoveSpeed": //64
					return val;

				case "Dangling": //0
					return val;

				case "Collide": //0
					return val;

				case "Breakable": //0
					return val;

				case "Barbed": //0
					return val;

				case "targetname": //ctspawn1
					return val;

				case "start_active": //1
					return val;

				case "effect_name": //ambient_leaf_blow_e
					return val;

				case "cpoint7_parent": //0
					return val;

				case "cpoint6_parent": //0
					return val;

				case "cpoint5_parent": //0
					return val;

				case "cpoint4_parent": //0
					return val;

				case "cpoint3_parent": //0
					return val;

				case "cpoint2_parent": //0
					return val;

				case "cpoint1_parent": //0
					return val;

				case "angles": //0 0 0
					return val;

				case "skin": //1
					return val;

				case "rendercolor": //255 255 255
					return val;

				case "renderamt": //255
					return val;

				case "pressuredelay": //0
					return val;

				case "physdamagescale": //0.1
					return val;

				case "model": //models/props_junk/CinderBlock01a.mdl
					return val;

				case "minhealthdmg": //0
					return val;

				case "massScale": //0
					return val;

				case "inertiaScale": //1
					return val;

				case "forcetoenablemotion": //0
					return val;

				case "fadescale": //1
					return val;

				case "fademindist": //-1
					return val;

				case "ExplodeRadius": //0
					return val;

				case "ExplodeDamage": //0
					return val;

				case "disableshadows": //1
					return val;

				case "Damagetype": //0
					return val;

				case "damagetoenablemotion": //0
					return val;

				case "shadowcastdist": //0
					return val;

				case "rendermode": //0
					return val;

				case "renderfx": //0
					return val;

				case "physicsmode": //0
					return val;

				case "PerformanceMode": //0
					return val;

				case "nodamageforces": //0
					return val;

				case "disablereceiveshadows": //0
					return val;

				case "ExploitableByPlayer": //0
					return val;

				case "fademaxdist": //0
					return val;

				case "shadowdepthnocache": //0
					return val;

				case "mingpulevel": //0
					return val;

				case "mincpulevel": //0
					return val;

				case "maxgpulevel": //0
					return val;

				case "maxcpulevel": //0
					return val;

				case "drawinfastreflection": //0
					return val;

				case "disableX360": //0
					return val;

				case "disableshadowdepth": //0
					return val;

				case "disableflashlight": //0
					return val;

				case "body": //0
					return val;

				case "petpopulation": //10
					return val;

				case "bombradius": //500
					return val;

				case "use_angles": //0
					return val;

				case "fogstart": //512
					return val;

				case "fogmaxdensity": //.2
					return val;

				case "foglerptime": //0
					return val;

				case "fogend": //5500
					return val;

				case "fogenable": //1
					return val;

				case "fogdir": //1 0 0
					return val;

		

				case "fogblend": //0
					return val;

				case "farz": //-1
					return val;

				case "minfalloff": //0.0
					return val;

				case "maxweight": //1.0
					return val;

				case "maxfalloff": //0
					return val;

				case "filename": //materials/correction/cc_italy.raw
					return val;

				case "fadeOutDuration": //0.0
					return val;

				case "fadeInDuration": //0.0
					return val;

				case "vignettestart": //0.8
					return val;

				case "vignetteend": //1.1
					return val;

				case "vignetteblurstrength": //0
					return val;

				case "screenblurstrength": //0
					return val;

				case "fadetoblackstrength": //0
					return val;

				case "fadetime": //2
					return val;

				case "depthblurstrength": //0
					return val;

				case "depthblurfocaldistance": //0
					return val;

				case "minwind": //5
					return val;

				case "mingustdelay": //10
					return val;

				case "mingust": //5
					return val;

				case "maxwind": //5
					return val;

				case "maxgustdelay": //20
					return val;

				case "maxgust": //5
					return val;

				case "gustduration": //5
					return val;

				case "gustdirchange": //20
					return val;

				case "style": //0
					return val;

				case "pitch": //-45
					return val;

				case "_zero_percent_distance": //0
					return val;

				case "_quadratic_attn": //1
					return val;

				case "_linear_attn": //0
					return val;

				case "_lightscaleHDR": //1
					return val;

				case "_lightHDR": //-1 -1 -1 1
					return val;

				case "_light": //194 215 220 50
					return val;

				case "_inner_cone": //1
					return val;

				case "_hardfalloff": //0
					return val;

				case "_fifty_percent_distance": //0
					return val;

				case "_exponent": //1
					return val;

				case "_distance": //0
					return val;

				case "_constant_attn": //0
					return val;

				case "_cone": //35
					return val;

				case "Negated": //Allow entities that match criteria
					return val;

				case "StartDisabled": //0
					return val;

				case "filtername": //InstanceAuto1-filter_chicken
					return val;

				case "scale": //.35
					return val;

				case "HDRColorScale": //1.0
					return val;

				case "GlowProxySize": //10
					return val;

				case "framerate": //10.0
					return val;



				case "surfacetype": //0
					return val;

				case "spawnobject": //0
					return val;

				case "propdata": //0
					return val;

				case "material": //0
					return val;


				case "health": //1
					return val;

				case "gibdir": //0 0 0
					return val;

				case "fragility": //100
					return val;

				case "explosion": //0
					return val;

				case "explodemagnitude": //0
					return val;

				case "error": //0
					return val;

				case "additionaliterations": //0
					return val;

				case "zmin": //0
					return val;

				case "zmax": //0
					return val;

				case "zfriction": //0
					return val;

				case "ymin": //0
					return val;

				case "ymax": //0
					return val;

				case "yfriction": //0
					return val;

				case "xmin": //-90
					return val;

				case "xmax": //90
					return val;

				case "xfriction": //0
					return val;

				case "torquelimit": //0
					return val;

				case "teleportfollowdistance": //0
					return val;

				case "forcelimit": //0
					return val;

				case "constraintsystem": //InstanceAuto1-chicken_system1
					return val;

				case "attach2": //InstanceAuto1-chicken1
					return val;

				case "attach1": //InstanceAuto1-brush1
					return val;

				case "vrad_brush_cast_shadows": //0
					return val;

				case "Solidity": //0
					return val;

				case "solidbsp": //0
					return val;

				case "invert_exclusion": //0
					return val;

				case "InputFilter": //0
					return val;

				case "SunSpreadAngle": //0
					return val;

				case "_AmbientScaleHDR": //1
					return val;

				case "_ambientHDR": //-1 -1 -1 1
					return val;

				case "_ambient": //169 193 205 120
					return val;

				case "nodeid": //1
					return val;

				case "portalnumber": //1
					return val;

				case "StartOpen": //1
					return val;

				case "PortalVersion": //1
					return val;

				case "OnBreak": //InstanceAuto1-operaWavStopSound0-1
					return val;

				case "distance": //72
					return val;

				case "disableallshadows": //0
					return val;



				case "TeamNum": //3
					return val;

				case "solid": //0
					return val;

				case "SetBodyGroup": //0
					return val;

				case "RandomAnimation": //0
					return val;

				case "MinAnimTime": //5
					return val;

				case "MaxAnimTime": //10
					return val;

				case "DefaultAnim": //sway
					return val;

				case "OnMapSpawn": //InstanceAuto1-tonemap_globalSetAutoExposureMin.50-1
					return val;

				case "size": //16
					return val;

				case "overlaysize": //-1
					return val;

				case "overlaymaterial": //sprites/light_glow02_add_noz
					return val;


				case "radius": //50
					return val;

				case "MainSoundscapeName": //ambient_cellarhouse
					return val;

				case "soundscape": //italy.indoor_b
					return val;

				case "position3": //ambient_outdoors_p3
					return val;

				case "position2": //ambient_outdoors_p2
					return val;

				case "position1": //ambient_outdoors_p1
					return val;

				case "position0": //ambient_outdoors_p0
					return val;

				case "priority": //1
					return val;
 */
				default:
					//if (!knownProps.ContainsKey(key))
					//{
					//    knownProps[key] = val;
					//    Trace.WriteLine(string.Format("case \""+key+"\": //"+val+"\n\treturn val;\n"));
					//}
					return val;
			}
		}

		protected int EvalNumItems(long total, long structSize)
		{
			if (total % structSize != 0)
			{
				throw new BspFormatException(string.Format("BSP entry size {0} should be power of {1}", total, structSize));
			}
			return (int)(total / structSize);
		}

		protected virtual void ReadEdges()
		{
		}

		protected virtual void ReadEffects()
		{
		}

		protected virtual void ReadEntities()
		{
		}

		protected virtual void ReadFaces()
		{
		}

		protected abstract void ReadHeader();

		protected virtual void ReadLeaves()
		{
		}

		protected virtual void ReadLightmaps()
		{
		}

		protected virtual void ReadModels()
		{
		}

		protected virtual void ReadNodes()
		{
		}

		protected virtual void ReadPlanes()
		{
		}

		protected virtual void ReadTextures()
		{
		}

		protected abstract void ReadVertices();

		protected virtual void ReadVisibilityList()
		{
		}

		protected void SeekEntryAt(long offset)
		{
			this.Stream.Position = this.startOfTheFile + offset;
		}

		private Color ParseColorEntityProperty(string val)
		{
			var v = val.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			byte r = byte.Parse(v[0], CultureInfo.InvariantCulture);
			byte g = byte.Parse(v[1], CultureInfo.InvariantCulture);
			byte b = byte.Parse(v[2], CultureInfo.InvariantCulture);
			return Color.FromArgb(255, r, g, b);
		}

		private Vector3 ParseVec3EntityProperty(string val)
		{
			var v = val.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			Vector3 res = new Vector3();
			res.X = float.Parse(v[0], CultureInfo.InvariantCulture);
			res.Y = float.Parse(v[1], CultureInfo.InvariantCulture);
			res.Z = float.Parse(v[2], CultureInfo.InvariantCulture);
			return res;
		}

		#endregion
	}
}
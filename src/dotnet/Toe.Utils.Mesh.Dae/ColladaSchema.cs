using System.Xml.Linq;

namespace Toe.Utils.Mesh.Dae
{
	public class ColladaSchema
	{
		#region Constants and Fields

		public XName accessorName;

		public XName ambientName;

		public XName assetName;

		public XName bindMaterialName;

		public XName bindVertexInputName;

		public XName blinnName;

		public XName colladaName;

		public string colladaSchema;

		public XName colorName;

		public XName constantName;

		public XName controllerName;

		public XName countName;

		public XName diffuseName;

		public XName effectName;

		public XName floatArrayName;

		public XName geometryName;

		public XName idName;

		public XName imageName;

		public XName initFromName;

		public XName inputName;

		public XName instanceControllerName;

		public XName instanceEffectName;

		public XName instanceGeometryName;

		public XName instanceMaterialName;

		public XName instanceVisualSceneName;

		public XName lambertName;

		public XName libraryAnimationsName;

		public XName libraryControllersName;

		public XName libraryEffectsName;

		public XName libraryGeometriesName;

		public XName libraryImagesName;

		public XName libraryLightsName;

		public XName libraryMaterialsName;

		public XName libraryVisualScenesName;

		public XName lightName;

		public XName materialAttributeName;

		public XName materialName;

		public XName meshName;

		public XName nameName;

		public XName newparamName;

		public XName nodeName;

		public XName offsetName;

		public XName pName;

		public XName paramName;

		public XName phongName;

		public XName polygonsName;

		public XName polylistName;

		public XName profileCommonName;

		public XName rotateName;

		public XName sampler2DName;

		public XName sceneName;

		public XName semanticName;

		public XName setName;

		public XName sidName;

		public XName skeletonName;

		public XName skinName;

		public XName sourceAttributeName;

		public XName sourceName;

		public XName strideName;

		public XName surfaceName;

		public XName symbolName;

		public XName targetName;

		public XName techniqueCommonName;

		public XName techniqueName;

		public XName textureAttributeName;

		public XName textureName;

		public XName trianglesName;

		public XName typeName;

		public XName urlName;

		public XName matrixName;

		public XName vcountName;

		public XName verticesName;

		public XName visualSceneName;

		private static ColladaSchema schema14;

		private static ColladaSchema schema15;

		#endregion

		#region Constructors and Destructors

		public ColladaSchema(string url)
		{
			this.colladaSchema = url;

			this.colladaName = XName.Get("COLLADA", this.colladaSchema);
			this.assetName = XName.Get("asset", this.colladaSchema);
			this.geometryName = XName.Get("geometry", this.colladaSchema);
			this.materialName = XName.Get("material", this.colladaSchema);
			this.materialAttributeName = XName.Get("material", "");

			this.effectName = XName.Get("effect", this.colladaSchema);
			this.meshName = XName.Get("mesh", this.colladaSchema);
			this.sourceName = XName.Get("source", this.colladaSchema);
			this.sourceAttributeName = XName.Get("source", "");
			this.floatArrayName = XName.Get("float_array", this.colladaSchema);
			this.techniqueCommonName = XName.Get("technique_common", this.colladaSchema);
			this.accessorName = XName.Get("accessor", this.colladaSchema);
			this.paramName = XName.Get("param", this.colladaSchema);
			this.ambientName = XName.Get("ambient", this.colladaSchema);
			this.colorName = XName.Get("color", this.colladaSchema);
			this.lightName = XName.Get("light", this.colladaSchema);
			this.rotateName = XName.Get("rotate", this.colladaSchema);
			this.matrixName = XName.Get("matrix", this.colladaSchema);
			this.nodeName = XName.Get("node", this.colladaSchema);
			this.instanceGeometryName = XName.Get("instance_geometry", this.colladaSchema);
			this.instanceControllerName = XName.Get("instance_controller", this.colladaSchema);
			this.instanceEffectName = XName.Get("instance_effect", this.colladaSchema);
			this.profileCommonName = XName.Get("profile_COMMON", this.colladaSchema);
			this.techniqueName = XName.Get("technique", this.colladaSchema);
			this.phongName = XName.Get("phong", this.colladaSchema);
			this.lambertName = XName.Get("lambert", this.colladaSchema);
			this.blinnName = XName.Get("blinn", this.colladaSchema);
			this.constantName = XName.Get("constant", this.colladaSchema);
			this.diffuseName = XName.Get("diffuse", this.colladaSchema);
			this.instanceMaterialName = XName.Get("instance_material", this.colladaSchema);
			this.bindVertexInputName = XName.Get("bind_vertex_input", this.colladaSchema);
			this.bindMaterialName = XName.Get("bind_material", this.colladaSchema);
			this.textureName = XName.Get("texture", this.colladaSchema);
			this.sampler2DName = XName.Get("sampler2D", this.colladaSchema);
			this.surfaceName = XName.Get("surface", this.colladaSchema);
			this.initFromName = XName.Get("init_from", this.colladaSchema);
			this.visualSceneName = XName.Get("visual_scene", this.colladaSchema);

			this.textureAttributeName = XName.Get("texture", "");
			this.symbolName = XName.Get("symbol", "");
			this.targetName = XName.Get("target", "");
			this.vcountName = XName.Get("vcount", this.colladaSchema);
			this.newparamName = XName.Get("newparam", this.colladaSchema);

			this.verticesName = XName.Get("vertices", this.colladaSchema);
			this.inputName = XName.Get("input", this.colladaSchema);
			this.semanticName = XName.Get("semantic", "");
			this.polygonsName = XName.Get("polygons", this.colladaSchema);
			this.trianglesName = XName.Get("triangles", this.colladaSchema);
			this.polylistName = XName.Get("polylist", this.colladaSchema);
			this.setName = XName.Get("set", "");
			this.pName = XName.Get("p", this.colladaSchema);
			this.idName = XName.Get("id", "");
			this.sidName = XName.Get("sid", "");
			this.offsetName = XName.Get("offset", "");
			this.countName = XName.Get("count", "");
			this.nameName = XName.Get("name", "");
			this.strideName = XName.Get("stride", "");
			this.typeName = XName.Get("type", "");
			this.imageName = XName.Get("image", this.colladaSchema);

			this.controllerName = XName.Get("controller", this.colladaSchema);
			this.skinName = XName.Get("skin", this.colladaSchema);
			this.skeletonName = XName.Get("skeleton", this.colladaSchema);
			this.libraryControllersName = XName.Get("library_controllers", this.colladaSchema);
			this.libraryAnimationsName = XName.Get("library_animations", this.colladaSchema);
			this.libraryImagesName = XName.Get("library_images", this.colladaSchema);
			this.libraryMaterialsName = XName.Get("library_materials", this.colladaSchema);
			this.libraryEffectsName = XName.Get("library_effects", this.colladaSchema);
			this.libraryGeometriesName = XName.Get("library_geometries", this.colladaSchema);
			this.libraryAnimationsName = XName.Get("library_animations", this.colladaSchema);
			this.libraryLightsName = XName.Get("library_lights", this.colladaSchema);
			this.libraryVisualScenesName = XName.Get("library_visual_scenes", this.colladaSchema);
			this.instanceVisualSceneName = XName.Get("instance_visual_scene", this.colladaSchema);
			this.urlName = XName.Get("url", "");

			this.sceneName = XName.Get("scene", this.colladaSchema);
		}

		#endregion

		#region Public Properties

		public static ColladaSchema Schema14
		{
			get
			{
				if (schema14 == null)
				{
					schema14 = new ColladaSchema("http://www.collada.org/2005/11/COLLADASchema");
				}
				return schema14;
			}
		}

		public static ColladaSchema Schema15
		{
			get
			{
				if (schema15 == null)
				{
					schema15 = new ColladaSchema("http://www.collada.org/2008/03/COLLADASchema");
				}
				return schema15;
			}
		}

		#endregion
	}
}
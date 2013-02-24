using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using OpenTK;

namespace Toe.Utils.Mesh.Dae
{
	public class DaeReader : ISceneReader
	{
		#region Constants and Fields

		private XElement collada;

		private ColladaSchema schema;

		private string basePath;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Load mesh from stream.
		/// </summary>
		/// <param name="stream">Stream to read from.</param>
		/// <param name="basePath"> </param>
		/// <returns>Complete parsed mesh.</returns>
		public IScene Load(Stream stream, string basePath)
		{
			this.basePath = basePath ?? Directory.GetCurrentDirectory();
			var scene = new Scene();

			var doc = XDocument.Load(stream);
			var r = doc.Root;
			this.schema = new ColladaSchema(r.Name.Namespace.NamespaceName);
			this.collada = doc.Element(this.schema.colladaName);
			this.ParseAsset(scene, this.collada.Element(this.schema.assetName));
			this.ParseImages(scene, this.collada.Element(this.schema.libraryImagesName));
			this.ParseEffects(scene, this.collada.Element(this.schema.libraryEffectsName));
			this.ParseMaterials(scene, this.collada.Element(this.schema.libraryMaterialsName));
			this.ParseGeometries(scene, this.collada.Element(this.schema.libraryGeometriesName));
			this.ParseScene(scene, this.collada, this.collada.Element(this.schema.sceneName));
			return scene;
		}

		#endregion

		#region Methods

		private static Vector3 GetVector3(int index, int stride, Input input, int[] poligonArray)
		{
			return input.SourceData.GetVector3(poligonArray[index * stride + input.Offset]);
		}

		private static void SetAvailableStreams(VertexBufferMesh dstMesh, MeshInputMap meshInputs)
		{
			dstMesh.IsNormalStreamAvailable |= meshInputs.Normal != null;
			dstMesh.IsUV0StreamAvailable |= meshInputs.TexCoord0 != null;
			dstMesh.IsUV1StreamAvailable |= meshInputs.TexCoord1 != null;
			dstMesh.IsColorStreamAvailable |= meshInputs.Color != null;
			dstMesh.IsTangentStreamAvailable |= meshInputs.Tangent != null;
			dstMesh.IsBinormalStreamAvailable |= meshInputs.Binormal != null;
		}

		private Color GetColor(int index, int stride, Input input, int[] poligonArray)
		{
			return input.SourceData.GetColor(poligonArray[index * stride + input.Offset]);
		}

		private void ParseAsset(Scene scene, XElement element)
		{
			if (element == null)
			{
				return;
			}
		}

		private Color ParseColor(string value)
		{
			var values = (from c in value.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
			              select (int)(255 * float.Parse(c, CultureInfo.InvariantCulture))).ToArray();
			if (values.Length == 0)
			{
				return Color.White;
			}
			if (values.Length <= 1)
			{
				return Color.FromArgb(255, values[0], values[0], values[0]);
			}
			if (values.Length <= 2)
			{
				return Color.FromArgb(255, values[0], values[1], values[0]);
			}
			if (values.Length <= 3)
			{
				return Color.FromArgb(255, values[0], values[1], values[2]);
			}
			return Color.FromArgb(values[3], values[0], values[1], values[2]);
		}

		private IColorSource ParseColorSource(Scene scene, XElement element)
		{
			var texture = element.Element(this.schema.textureName);
			if (texture != null)
			{
				var imageColorSource = new ImageColorSource();
				var texAttr = texture.Attribute(this.schema.textureAttributeName);
				if (texAttr != null)
				{
					var sampler = (from p1 in element.Parent.Parent.Parent.Elements()
					               where
					               	p1.Name == this.schema.newparamName && p1.AttributeValue(this.schema.sidName) == texAttr.Value
					               select p1).FirstOrDefault();
					if (sampler != null)
					{
						// texture reference through sampler
						var sampler2D = sampler.Element(this.schema.sampler2DName);
						if (sampler2D != null)
						{
							var sampler2DSource = sampler2D.Element(this.schema.sourceName);
							var surfaceParam = (from p1 in element.Parent.Parent.Parent.Elements()
							                    where
							                    	p1.Name == this.schema.newparamName
							                    	&& p1.AttributeValue(this.schema.sidName) == sampler2DSource.Value
							                    select p1).First();
							var surface = surfaceParam.Element(this.schema.surfaceName);
							var imageId = surface.ElementValue(this.schema.initFromName);
							foreach (var i in scene.Images.Where(i => i.Id == imageId))
							{
								imageColorSource.Image = i;
								break;
							}
						}
					}
					else
					{
						// texture reference through url
						foreach (var i in scene.Images.Where(i => i.Id == texAttr.Value))
						{
							imageColorSource.Image = i;
							break;
						}
					}
				}
				return imageColorSource;
			}
			var color = element.Element(this.schema.colorName);
			if (color != null)
			{
				var solidColorSource = new SolidColorSource { Color = this.ParseColor(color.Value) };

				return solidColorSource;
			}
			return null;
		}

		private void ParseEffect(Scene scene, XElement effect)
		{
			var sceneEffect = new SceneEffect();
			this.ParseIdAndName(effect, sceneEffect);
			var profileCommon = effect.Element(this.schema.profileCommonName);
			if (profileCommon != null)
			{
				var technique = profileCommon.Element(this.schema.techniqueName);
				if (technique != null)
				{
					var techniqueImpl = technique.Element(this.schema.phongName);
					if (techniqueImpl == null)
					{
						techniqueImpl = technique.Element(this.schema.lambertName);
						if (techniqueImpl == null)
						{
							techniqueImpl = technique.Element(this.schema.blinnName);
							if (techniqueImpl == null)
							{
								techniqueImpl = technique.Element(this.schema.constantName);
								if (techniqueImpl == null)
								{
									throw new DaeException(string.Format("Unknow technique at {0}", scene.Name));
								}
							}
						}
					}
					sceneEffect.Diffuse = this.ParseColorSource(scene, techniqueImpl.Element(this.schema.diffuseName));
				}
			}
			scene.Effects.Add(sceneEffect);
		}

		private void ParseEffects(Scene scene, XElement element)
		{
			if (element == null)
			{
				return;
			}
			foreach (var effect in element.Elements(this.schema.effectName))
			{
				this.ParseEffect(scene, effect);
			}
		}

		private void ParseElement(
			int[] poligonArray, VertexBufferSubmesh subMesh, MeshInputMap meshInputs, ref Vertex vertex, int index)
		{
			if (meshInputs.Vertex != null)
			{
				vertex.Position = GetVector3(index, meshInputs.Stride, meshInputs.Vertex, poligonArray);
			}
			if (meshInputs.Normal != null)
			{
				vertex.Normal = GetVector3(index, meshInputs.Stride, meshInputs.Normal, poligonArray);
			}
			if (meshInputs.TexCoord0 != null)
			{
				var vector3 = GetVector3(index, meshInputs.Stride, meshInputs.TexCoord0, poligonArray);
				vertex.UV0 = new Vector3(vector3.X,1.0f-vector3.Y,vector3.Z);
			}
			if (meshInputs.TexCoord1 != null)
			{
				var vector3 = GetVector3(index, meshInputs.Stride, meshInputs.TexCoord1, poligonArray);
				vertex.UV1 = new Vector3(vector3.X, 1.0f - vector3.Y, vector3.Z);
			}
			if (meshInputs.Color != null)
			{
				vertex.Color = this.GetColor(index, meshInputs.Stride, meshInputs.Color, poligonArray);
			}
			subMesh.Add(ref vertex);
		}

		private void ParseElements(int[] poligonArray, VertexBufferSubmesh subMesh, MeshInputMap meshInputs, int numElements)
		{
			Vertex vertex = new Vertex();
			for (int index = 0; index < numElements; ++index)
			{
				this.ParseElement(poligonArray, subMesh, meshInputs, ref vertex, index);
			}
		}

		private void ParseGeometries(Scene scene, XElement element)
		{
			if (element == null)
			{
				return;
			}
			foreach (var geometry in element.Elements(this.schema.geometryName))
			{
				this.ParseGeometry(scene, geometry);
			}
		}

		private void ParseGeometry(Scene scene, XElement geometry)
		{
			var skinAndMaterials = FindContext(scene, geometry);

			var mesh = geometry.Element(this.schema.meshName);

			var dstMesh = new VertexBufferMesh();
			this.ParseIdAndName(geometry, dstMesh);
			scene.Geometries.Add(dstMesh);
			if (mesh != null)
			{
				this.ParseMesh(scene, mesh, skinAndMaterials, dstMesh);
				return;
			}
			throw new DaeException(
				string.Format("Geometry type at {0} not supported yet.", geometry.AttributeValue(this.schema.idName)));
		}

		private MeshSkinAndMaterials FindContext(Scene scene, XElement geometry)
		{
			var sam = new MeshSkinAndMaterials();

			var geometryId = "#" + geometry.AttributeValue(schema.idName);

			var controllers = collada.Element(schema.libraryControllersName);
			if (controllers != null)
			{
				sam.SkinController = (from controller in controllers.Elements(schema.controllerName)
				let skin = controller.Element(schema.skinName)
									  where skin != null && skin.AttributeValue(schema.sourceAttributeName) == geometryId
				select controller).FirstOrDefault();
			}

			string controllerId = "#";

			if (sam.SkinController != null)
				controllerId = "#" + sam.SkinController.AttributeValue(schema.idName);

			var libScenes = collada.Element(schema.libraryVisualScenesName);
			if (libScenes != null)
			{
				foreach (var visualScene in libScenes.Elements(schema.visualSceneName))
				{
					foreach (var node in visualScene.Descendants(schema.nodeName))
					{
						var instanceGeo = node.Element(schema.instanceGeometryName);
						if (instanceGeo != null && instanceGeo.AttributeValue(schema.urlName) == geometryId)
						{
							CopySkin(scene, instanceGeo, sam);
						}
						var instanceCtrl = node.Element(schema.instanceControllerName);
						if (instanceCtrl != null && instanceCtrl.AttributeValue(schema.urlName) == controllerId)
						{
							CopySkin(scene, instanceCtrl, sam);
						}
					}
				}
			}
			return sam;
		}

		private void CopySkin(Scene scene, XElement instance, MeshSkinAndMaterials sam)
		{
			var bind = instance.Element(schema.bindMaterialName);
			if(bind != null)
			{
				var technique_common = bind.Element(schema.techniqueCommonName);
				if (technique_common != null)
				{
					var ms = new MaterialSet();
					foreach (var instanceMaterial in technique_common.Elements(schema.instanceMaterialName))
					{
						var symbol = instanceMaterial.AttributeValue(schema.symbolName);
						var target = instanceMaterial.AttributeValue(schema.targetName);
						var matId = collada.Element(schema.libraryMaterialsName).ElementByUrl(target).AttributeValue(schema.idName);
						ms.Add(symbol, (from m in scene.Materials where m.Id == matId select m).First());
					}
					sam.Materials.Add(ms);
				}
			}
		}

		private void ParseIdAndName(XElement effect, ISceneItem sceneEffect)
		{
			var idAttr = effect.Attribute(this.schema.idName);
			if (idAttr != null)
			{
				sceneEffect.Id = idAttr.Value;
			}
			var nameAttr = effect.Attribute(this.schema.nameName);
			sceneEffect.Name = (nameAttr != null) ? nameAttr.Value : sceneEffect.Id;
		}

		private void ParseImage(Scene scene, XElement image)
		{
			var initFrom = image.ElementValue(schema.initFromName);
			if (!string.IsNullOrEmpty(initFrom))
			{
				var fileReferenceImage = new FileReferenceImage() { Path = Path.Combine(basePath, initFrom) };
				this.ParseIdAndName(image, fileReferenceImage);
				scene.Images.Add(fileReferenceImage);
				return;
			}
			var embeddedImage = new EmbeddedImage();
			this.ParseIdAndName(image, embeddedImage);
			scene.Images.Add(embeddedImage);
		}

		private void ParseImages(Scene scene, XElement element)
		{
			if (element == null)
			{
				return;
			}
			foreach (var image in element.Elements(this.schema.imageName))
			{
				this.ParseImage(scene, image);
			}
		}

		private void ParseInputs(MeshInputMap meshInputs, XElement src, XElement mesh)
		{
			meshInputs.Count = int.Parse(src.AttributeValue(this.schema.countName), CultureInfo.InvariantCulture);
			foreach (var input in src.Elements(this.schema.inputName))
			{
				var semantic = input.AttributeValue(this.schema.semanticName);
				var inputInfo = new Input(input, this.schema, mesh);
				if (meshInputs.Stride <= inputInfo.Offset)
				{
					meshInputs.Stride = inputInfo.Offset + 1;
				}
				if (0 == string.Compare(semantic, "VERTEX", StringComparison.InvariantCultureIgnoreCase))
				{
					meshInputs.Vertex = inputInfo;
				}
				else if (0 == string.Compare(semantic, "NORMAL", StringComparison.InvariantCultureIgnoreCase))
				{
					meshInputs.Normal = inputInfo;
				}
				else if (0 == string.Compare(semantic, "TEXCOORD", StringComparison.InvariantCultureIgnoreCase))
				{
					switch (inputInfo.Set)
					{
						case 0:
							meshInputs.TexCoord0 = inputInfo;
							break;
						case 1:
							meshInputs.TexCoord1 = inputInfo;
							break;
					}
				}
				else if (0 == string.Compare(semantic, "COLOR", StringComparison.InvariantCultureIgnoreCase))
				{
					switch (inputInfo.Set)
					{
						case 0:
							meshInputs.Color = inputInfo;
							break;
					}
				}
				else if (0 == string.Compare(semantic, "POSITION", StringComparison.InvariantCultureIgnoreCase))
				{
					meshInputs.Position = inputInfo;
				}
				else
				{
					throw new DaeException(string.Format(CultureInfo.InvariantCulture, "Unknown semantic {0}", semantic));
				}
			}
		}

		private void ParseIntArray(string polygon, int[] poligonArray)
		{
			var items = polygon.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			if (items.Length != poligonArray.Length)
			{
				throw new DaeException(string.Format("Array size doesn't match expected value {0}", poligonArray.Length));
			}
			for (int index = 0; index < items.Length; ++index)
			{
				poligonArray[index] = int.Parse(items[index], CultureInfo.InvariantCulture);
			}
		}

		private void ParseMaterial(Scene scene, XElement material)
		{
			var sceneMaterial = new SceneMaterial();
			this.ParseIdAndName(material, sceneMaterial);
			var instanceEffect = material.Element(this.schema.instanceEffectName);
			if (instanceEffect != null)
			{
				string id = instanceEffect.AttributeValue(this.schema.urlName).Substring(1);
				sceneMaterial.Effect = (from g in scene.Effects where g.Id == id select g).First();
			}
			scene.Materials.Add(sceneMaterial);
		}

		private void ParseMaterials(Scene scene, XElement element)
		{
			if (element == null)
			{
				return;
			}
			foreach (var material in element.Elements(this.schema.materialName))
			{
				this.ParseMaterial(scene, material);
			}
		}

		private void ParseMesh(Scene scene, XElement mesh, MeshSkinAndMaterials skinAndMaterials, VertexBufferMesh dstMesh)
		{
			foreach (var element in mesh.Elements())
			{
				if (element.Name == this.schema.polygonsName)
				{
					this.ParsePolygons(mesh, skinAndMaterials, dstMesh, element);
				}
				else if (element.Name == this.schema.polylistName)
				{
					this.ParsePolylist(mesh, skinAndMaterials, dstMesh, element);
				}
				else if (element.Name == this.schema.trianglesName)
				{
					this.ParseTriangles(mesh, skinAndMaterials, dstMesh, element);
				}
				else if (element.Name == this.schema.verticesName || element.Name == this.schema.sourceName)
				{
				}
				else
				{
					throw new DaeException(message: "Unknown mesh element " + element.Name.LocalName);
				}
			}
		}

		private void ParseNode(Scene scene, INodeContainer parent, XElement node)
		{
			var n = new Node();
			this.ParseIdAndName(node, n);

			var geo = node.Element(this.schema.instanceGeometryName);
			if (geo != null)
			{
				var url = geo.AttributeValue(this.schema.urlName);
				string id = url.Substring(1);
				n.Mesh = (from g in scene.Geometries where g.Id == id select g).First();
			}
			else
			{
				var ctrl = node.Element(this.schema.instanceControllerName);
				if (ctrl != null)
				{
					var url = ctrl.AttributeValue(this.schema.urlName);
					string id = url.Substring(1);
					var controller = this.collada.Element(this.schema.libraryControllersName).ElementById(id);
					url = controller.Element(this.schema.skinName).AttributeValue(this.schema.sourceAttributeName);
					id = url.Substring(1);
					n.Mesh = (from g in scene.Geometries where g.Id == id select g).First();
				}
			}

			parent.Nodes.Add(n);
		}

		private void ParseNodes(Scene scene, INodeContainer parent, IEnumerable<XElement> elements)
		{
			foreach (var node in elements)
			{
				this.ParseNode(scene, parent, node);
			}
		}

		private void ParsePolygons(XElement mesh, MeshSkinAndMaterials skinAndMaterials, VertexBufferMesh dstMesh, XElement element)
		{
			var meshInputs = new MeshInputMap();
			this.ParseInputs(meshInputs, element, mesh);

			var subMesh = (VertexBufferSubmesh)dstMesh.CreateSubmesh();
			subMesh.Material = skinAndMaterials.GetAnyMaterialFor(element.AttributeValue(schema.materialAttributeName));
			subMesh.VertexSourceType = VertexSourceType.TrianleList;

			SetAvailableStreams(dstMesh, meshInputs);

			int[] poligonArray = new int[3 * meshInputs.Stride];

			foreach (var polygon in element.Elements(this.schema.pName))
			{
				this.ParseIntArray(polygon.Value, poligonArray);

				this.ParseElements(poligonArray, subMesh, meshInputs, 3);
			}
		}

		private void ParsePolylist(XElement mesh, MeshSkinAndMaterials skinAndMaterials, VertexBufferMesh dstMesh, XElement element)
		{
			var meshInputs = new MeshInputMap();
			this.ParseInputs(meshInputs, element, mesh);
			SetAvailableStreams(dstMesh, meshInputs);
			int[] vcount = new int[meshInputs.Count];
			this.ParseIntArray(element.ElementValue(this.schema.vcountName), vcount);
			var points = vcount.Sum(a => a);
			var firstSize = vcount[0];
			bool isAllTheSame = vcount.All(i => i == firstSize);

			int[] p = new int[points * meshInputs.Stride];
			this.ParseIntArray(element.ElementValue(this.schema.pName), p);

			var subMesh = (VertexBufferSubmesh)dstMesh.CreateSubmesh();
			subMesh.Material = skinAndMaterials.GetAnyMaterialFor(element.AttributeValue(schema.materialAttributeName));
			if (isAllTheSame)
			{
				if (firstSize == 3)
				{
					subMesh.VertexSourceType = VertexSourceType.TrianleList;
					this.ParseElements(p, subMesh, meshInputs, points);
					return;
				}
				if (firstSize == 4)
				{
					subMesh.VertexSourceType = VertexSourceType.QuadList;
					this.ParseElements(p, subMesh, meshInputs, points);
					return;
				}
			}

			Vertex vertex = new Vertex();
			subMesh.VertexSourceType = VertexSourceType.TrianleList;
			int startIndex = 0;
			for (int polygonIndex = 0; polygonIndex < vcount.Length; ++polygonIndex)
			{
				for (int i = 2; i < vcount[polygonIndex]; i++)
				{
					this.ParseElement(p, subMesh, meshInputs, ref vertex, startIndex + 0);
					this.ParseElement(p, subMesh, meshInputs, ref vertex, startIndex + i - 1);
					this.ParseElement(p, subMesh, meshInputs, ref vertex, startIndex + i);
				}
				startIndex += vcount[polygonIndex];
			}
		}

		private void ParseScene(Scene scene, XElement collada, XElement element)
		{
			if (element == null)
			{
				return;
			}
			var instance_visual_scene = element.Element(this.schema.instanceVisualSceneName);
			if (instance_visual_scene == null)
			{
				return;
			}
			var url = instance_visual_scene.Attribute(this.schema.urlName);
			if (url == null)
			{
				return;
			}

			var sceneId = url.Value;

			var libraryVisualScenes = collada.Element(this.schema.libraryVisualScenesName);
			var visualScene = libraryVisualScenes.ElementById(sceneId.Substring(1));
			if (visualScene == null)
			{
				return;
			}

			this.ParseIdAndName(visualScene, scene);

			this.ParseNodes(scene, scene, visualScene.Elements(this.schema.nodeName));
		}

		private void ParseTriangles(XElement mesh, MeshSkinAndMaterials skinAndMaterials, VertexBufferMesh dstMesh, XElement element)
		{
			var meshInputs = new MeshInputMap();
			this.ParseInputs(meshInputs, element, mesh);
			SetAvailableStreams(dstMesh, meshInputs);
			int[] p = new int[meshInputs.Count * meshInputs.Stride * 3];
			this.ParseIntArray(element.ElementValue(this.schema.pName), p);

			var subMesh = (VertexBufferSubmesh)dstMesh.CreateSubmesh();
			subMesh.Material = skinAndMaterials.GetAnyMaterialFor(element.AttributeValue(schema.materialAttributeName));
			subMesh.VertexSourceType = VertexSourceType.TrianleList;

			this.ParseElements(p, subMesh, meshInputs, meshInputs.Count * 3);
		}

		#endregion
	}
}
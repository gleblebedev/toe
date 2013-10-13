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

		private string basePath;

		private XElement collada;

		private ColladaSchema schema;

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
			this.AssignNullMaterialToDefault(scene);
			return scene;
		}

		private void AssignNullMaterialToDefault(Scene scene)
		{
			IMaterial defaultMaterial = null;
			foreach (var geometry in scene.Geometries)
			{
				foreach (var submesh in geometry.Submeshes)
				{
					if (submesh.Material == null)
					{
						submesh.Material = (defaultMaterial = defaultMaterial
						                  ?? new SceneMaterial() { Effect = new SceneEffect() { CullMode = CullMode.Back } });
					}
				}
			}
		}

		#endregion

		#region Methods

		//private static Vector3 GetVector3(int index, int stride, Input input, int[] poligonArray)
		//{
		//	return input.SourceData.GetVector3(poligonArray[index * stride + input.Offset]);
		//}

		private void CopySkin(Scene scene, XElement instance, MeshSkinAndMaterials sam)
		{
			var bind = instance.Element(this.schema.bindMaterialName);
			if (bind != null)
			{
				var technique_common = bind.Element(this.schema.techniqueCommonName);
				if (technique_common != null)
				{
					var ms = new MaterialSet();
					foreach (var instanceMaterial in technique_common.Elements(this.schema.instanceMaterialName))
					{
						var symbol = instanceMaterial.AttributeValue(this.schema.symbolName);
						var target = instanceMaterial.AttributeValue(this.schema.targetName);
						var matId =
							this.collada.Element(this.schema.libraryMaterialsName).ElementByUrl(target).AttributeValue(this.schema.idName);
						ms.Add(symbol, (from m in scene.Materials where m.Id == matId select m).First());
					}
					sam.Materials.Add(ms);
				}
			}
		}

		private MeshSkinAndMaterials FindContext(Scene scene, XElement geometry)
		{
			var sam = new MeshSkinAndMaterials();

			var geometryId = "#" + geometry.AttributeValue(this.schema.idName);

			var controllers = this.collada.Element(this.schema.libraryControllersName);
			if (controllers != null)
			{
				sam.SkinController = (from controller in controllers.Elements(this.schema.controllerName)
				                      let skin = controller.Element(this.schema.skinName)
				                      where skin != null && skin.AttributeValue(this.schema.sourceAttributeName) == geometryId
				                      select controller).FirstOrDefault();
			}

			string controllerId = "#";

			if (sam.SkinController != null)
			{
				controllerId = "#" + sam.SkinController.AttributeValue(this.schema.idName);
			}

			var libScenes = this.collada.Element(this.schema.libraryVisualScenesName);
			if (libScenes != null)
			{
				foreach (var visualScene in libScenes.Elements(this.schema.visualSceneName))
				{
					foreach (var node in visualScene.Descendants(this.schema.nodeName))
					{
						var instanceGeo = node.Element(this.schema.instanceGeometryName);
						if (instanceGeo != null && instanceGeo.AttributeValue(this.schema.urlName) == geometryId)
						{
							this.CopySkin(scene, instanceGeo, sam);
						}
						var instanceCtrl = node.Element(this.schema.instanceControllerName);
						if (instanceCtrl != null && instanceCtrl.AttributeValue(this.schema.urlName) == controllerId)
						{
							this.CopySkin(scene, instanceCtrl, sam);
						}
					}
				}
			}
			return sam;
		}

		//private Color GetColor(int index, int stride, Input input, int[] poligonArray)
		//{
		//	return input.SourceData.GetColor(poligonArray[index * stride + input.Offset]);
		//}

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
					               where p1.Name == this.schema.newparamName && p1.AttributeValue(this.schema.sidName) == texAttr.Value
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

		//private void ParseElement(
		//	int[] poligonArray, SeparateStreamsSubmesh subMesh, ref Vertex vertex, int index)
		//{
		//	if (meshInputs.Vertex != null)
		//	{
				
		//		vertex.Position = GetVector3(index, meshInputs.Stride, meshInputs.Vertex, poligonArray);
		//	}
		//	if (meshInputs.Normal != null)
		//	{
		//		vertex.Normal = GetVector3(index, meshInputs.Stride, meshInputs.Normal, poligonArray);
		//	}
		//	if (meshInputs.TexCoord0 != null)
		//	{
		//		var vector3 = GetVector3(index, meshInputs.Stride, meshInputs.TexCoord0, poligonArray);
		//		vertex.UV0 = new Vector3(vector3.X, 1.0f - vector3.Y, vector3.Z);
		//	}
		//	if (meshInputs.TexCoord1 != null)
		//	{
		//		var vector3 = GetVector3(index, meshInputs.Stride, meshInputs.TexCoord1, poligonArray);
		//		vertex.UV1 = new Vector3(vector3.X, 1.0f - vector3.Y, vector3.Z);
		//	}
		//	if (meshInputs.Color != null)
		//	{
		//		vertex.Color = this.GetColor(index, meshInputs.Stride, meshInputs.Color, poligonArray);
		//	}
		//	subMesh.Add(ref vertex);
		//}

		//private void ParseElements(int[] poligonArray, SeparateStreamsSubmesh subMesh, int numElements)
		//{
		//	Vertex vertex = new Vertex();
		//	for (int index = 0; index < numElements; ++index)
		//	{
		//		this.ParseElement(poligonArray, subMesh, ref vertex, index);
		//	}
		//}

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
			var skinAndMaterials = this.FindContext(scene, geometry);

			var mesh = geometry.Element(this.schema.meshName);

			var dstMesh = new SeparateStreamsMesh();
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
			var initFrom = image.ElementValue(this.schema.initFromName);
			if (!string.IsNullOrEmpty(initFrom))
			{
				var fileReferenceImage = new FileReferenceImage { Path = Path.Combine(this.basePath, initFrom) };
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
		public static ISource ParseSource(XElement xElement, ColladaSchema schema)
		{
			var floatArray = xElement.Element(schema.floatArrayName);
			if (floatArray != null)
			{
				return ParseFloatArraySource(xElement, schema);
			}
			var intArray = xElement.Element(schema.intArrayName);
			if (intArray != null)
			{
				return ParseIntArraySource(xElement, schema);
			}
			var boolArray = xElement.Element(schema.boolArrayName);
			if (boolArray != null)
			{
				return ParseBoolArraySource(xElement, schema);
			}
			var nameArray = xElement.Element(schema.nameArrayName);
			if (nameArray != null)
			{
				return ParseNameArraySource(xElement, schema);
			}
			var idrefArray = xElement.Element(schema.boolArrayName);
			if (idrefArray != null)
			{
				return ParseIDREFArraySource(xElement, schema);
			}
			throw new DaeException(
				string.Format(CultureInfo.InvariantCulture, "Unknown source type {0}", xElement.Name));
		}
		private static ISource ParseFloatArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new FloatArraySource(xElement, schema);
			return s;
		}
		private static ISource ParseIntArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new IntArraySource(xElement, schema);
			return s;
		}
		private static ISource ParseBoolArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new BoolArraySource(xElement, schema);
			return s;
		}
		private static ISource ParseNameArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new NameArraySource(xElement, schema);
			return s;
		}
		private static ISource ParseIDREFArraySource(XElement xElement, ColladaSchema schema)
		{
			var s = new IDREFArraySource(xElement, schema);
			return s;
		}
		

		private void ParseFloatArray(string polygon, float[] poligonArray)
		{
			var items = polygon.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			if (items.Length != poligonArray.Length)
			{
				throw new DaeException(string.Format("Array size doesn't match expected value {0}", poligonArray.Length));
			}
			for (int index = 0; index < items.Length; ++index)
			{
				poligonArray[index] = float.Parse(items[index], CultureInfo.InvariantCulture);
			}
		}
		private IList<int> ParseIntList(string polygon)
		{
			var items = polygon.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			return items.Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToList();
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

		private void ParseMesh(Scene scene, XElement mesh, MeshSkinAndMaterials skinAndMaterials, SeparateStreamsMesh dstMesh)
		{
			Dictionary<string, StreamKey> knownStreams = new Dictionary<string, StreamKey>();
			Dictionary<StreamKey, string> knownSemantics = new Dictionary<StreamKey, string>();
			foreach (var element in mesh.Elements())
			{
				foreach (var subElement in element.Elements().Where(x=>x.Name == this.schema.inputName))
				{
					var semanticAttribute = subElement.Attribute(schema.semanticName);
					var semantic = (semanticAttribute == null)?null:semanticAttribute.Value;
					var setAttribute = subElement.Attribute(schema.setName);
					var set = (setAttribute==null)?0:int.Parse(setAttribute.Value, CultureInfo.InvariantCulture);
					var sourceAttribute = subElement.Attribute(schema.sourceAttributeName);
					var source = (sourceAttribute == null)?null:sourceAttribute.Value;
					var key = new StreamKey(semantic, set);
					StreamKey knownStreamKey;
					if (!knownStreams.TryGetValue(source, out knownStreamKey))
					{
						knownStreams.Add(source,key);
					}
					else if (knownStreamKey != key)
					{
						throw new NotImplementedException();
					}

					string knownSemanticsKey;
					if (!knownSemantics.TryGetValue(key, out knownSemanticsKey))
					{
						knownSemantics.Add(key, source);
					}
					else if (knownSemanticsKey != source)
					{
						throw new NotImplementedException();
					}
				}
			}
			foreach (var streamName in knownStreams)
			{
				var s = mesh.ElementByUrl(streamName.Key);
				if (s != null)
				{
					if (s.Name == schema.verticesName)
					{
						
					}
					else
					{
						var source = ParseSource(s, schema);
						dstMesh.SetStream(streamName.Value.Key, streamName.Value.Channel, CreateMeshStream(source, streamName.Value.Key));
					}
				}
			}
			foreach (var element in mesh.Elements())
			{
				if (element.Name == this.schema.polygonsName)
				{
					this.ParsePolygons(skinAndMaterials, dstMesh, element);
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

		private IMeshStream CreateMeshStream(ISource source, string semantic)
		{
			var floatArray = source as FloatArraySource;
			if (floatArray != null)
			{
				bool swapY = (semantic == Streams.TexCoord);
				if (source.GetStride() == 3)
				{
					var arrayMeshStream = new ArrayMeshStream<Vector3>(source.GetCount());
					for (int i = 0; i < arrayMeshStream.Count; ++i)
					{
						var y = floatArray[i * 3 + 1];
						if (swapY) y = 1.0f - y;
						arrayMeshStream[i] = new Vector3(floatArray[i * 3 + 0], y, floatArray[i * 3 + 2]);
					}
					return arrayMeshStream;
				}
				else if (source.GetStride() == 2)
				{
					var arrayMeshStream = new ArrayMeshStream<Vector2>(source.GetCount());
					for (int i = 0; i < arrayMeshStream.Count; ++i)
					{
						var y = floatArray[i * 2 + 1];
						if (swapY) y = 1.0f - y;
						arrayMeshStream[i] = new Vector2(floatArray[i * 2 + 0], y);
					}
					return arrayMeshStream;
				}
				else if (source.GetStride() == 4)
				{
					var arrayMeshStream = new ArrayMeshStream<Vector4>(source.GetCount());
					for (int i = 0; i < arrayMeshStream.Count; ++i)
					{
						var y = floatArray[i * 4 + 1];
						if (swapY) y = 1.0f - y;
						arrayMeshStream[i] = new Vector4(floatArray[i * 4 + 0], y, floatArray[i * 4 + 2], floatArray[i * 4 + 3]);
					}
					return arrayMeshStream;
				}
			}
			else
			{
				throw new NotImplementedException();
			}
			throw new NotImplementedException();
		}

		private void ParseNode(Scene scene, INodeContainer parent, XElement node)
		{
			var n = new Node();
			this.ParseIdAndName(node, n);
			var mat = this.ParseMatrix(node.Element(this.schema.matrixName));
			n.Position = new Vector3(mat.M14, mat.M24, mat.M34);
			//n.Rotation = 

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

		private Matrix4 ParseMatrix(XElement element)
		{
			if (element == null) return Matrix4.Identity;
			var array = new float[16];
			this.ParseFloatArray(element.Value, array);
			return new Matrix4(array[0], array[1], array[2], array[3], 
				array[4], array[5], array[6], array[7], 
				array[8], array[9], array[10], array[11], 
				array[12], array[13], array[14], array[15] );
		}

		private void ParseNodes(Scene scene, INodeContainer parent, IEnumerable<XElement> elements)
		{
			foreach (var node in elements)
			{
				this.ParseNode(scene, parent, node);
			}
		}

		private void ParsePolygons( MeshSkinAndMaterials skinAndMaterials, SeparateStreamsMesh dstMesh, XElement element)
		{
			var meshInputs =  this.ParseInputs(element);
			var subMesh = dstMesh.CreateSubmesh();
			subMesh.Material = skinAndMaterials.GetAnyMaterialFor(element.AttributeValue(this.schema.materialAttributeName));
			subMesh.VertexSourceType = VertexSourceType.TriangleList;

			var streamList = StreamListOrderedByOffset(meshInputs, subMesh);
			foreach (var polygon in element.Elements(this.schema.pName))
			{
				var list = this.ParseIntList(polygon.Value);
				int numElements = list.Count / meshInputs.Stride;

				for (int i=1; i<numElements-1; ++i)
				{
					AddVertex(list, 0, streamList, meshInputs.Stride);
					AddVertex(list, i, streamList, meshInputs.Stride);
					AddVertex(list, i+1, streamList, meshInputs.Stride);
				}
			}
		}

		private static List<Tuple<int, ListMeshStream<int>>> StreamListOrderedByOffset(MeshInputs meshInputs, SeparateStreamsSubmesh subMesh)
		{
			var streamList = new List<Tuple<int, ListMeshStream<int>>>();
			foreach (var meshInput in meshInputs.Inputs.OrderBy(x => x.Offset))
			{
				var listMeshStream = new ListMeshStream<int>(meshInputs.Count);
				streamList.Add(new Tuple<int, ListMeshStream<int>>(meshInput.Offset, listMeshStream));
				var key = meshInput.Semantic;
				if (key == "VERTEX") key = Streams.Position;
				subMesh.SetIndexStream(key, meshInput.Set, listMeshStream);
			}
			return streamList;
		}

		private void AddVertex(IList<int> list, int i, List<Tuple<int, ListMeshStream<int>>> lists, int stride)
		{
			foreach (var stream in lists)
			{
				stream.Item2.Add(list[i*stride+stream.Item1]);
			}
		}

		private MeshInputs ParseInputs(XElement element)
		{
			return new MeshInputs(element,schema);
		}

		private void ParsePolylist(
			XElement mesh, MeshSkinAndMaterials skinAndMaterials, SeparateStreamsMesh dstMesh, XElement element)
		{
			var meshInputs = this.ParseInputs(element);

			
			int[] vcount = new int[meshInputs.Count];
			this.ParseIntArray(element.ElementValue(this.schema.vcountName), vcount);
			var points = vcount.Sum(a => a);
			var firstSize = vcount[0];
			bool isAllTheSame = vcount.All(i => i == firstSize);

			int[] p = new int[points * meshInputs.Stride];
			this.ParseIntArray(element.ElementValue(this.schema.pName), p);

			var subMesh = dstMesh.CreateSubmesh();
			subMesh.Material = skinAndMaterials.GetAnyMaterialFor(element.AttributeValue(this.schema.materialAttributeName));

			var streamList = StreamListOrderedByOffset(meshInputs, subMesh);

			//if (isAllTheSame)
			//{
			//	if (firstSize == 3)
			//	{
			//		subMesh.VertexSourceType = VertexSourceType.TrianleList;
			//		this.ParseElements(p, subMesh, meshInputs, points);
			//		return;
			//	}
			//	if (firstSize == 4)
			//	{
			//		subMesh.VertexSourceType = VertexSourceType.QuadList;
			//		this.ParseElements(p, subMesh, meshInputs, points);
			//		return;
			//	}
			//}

			subMesh.VertexSourceType = VertexSourceType.TriangleList;
			int startIndex = 0;
			for (int polygonIndex = 0; polygonIndex < vcount.Length; ++polygonIndex)
			{
				for (int i = 2; i < vcount[polygonIndex]; i++)
				{
					AddVertex(p, 0, streamList, meshInputs.Stride);
					AddVertex(p, i-1, streamList, meshInputs.Stride);
					AddVertex(p, i , streamList, meshInputs.Stride);
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

		private void ParseTriangles(
			XElement mesh, MeshSkinAndMaterials skinAndMaterials, SeparateStreamsMesh dstMesh, XElement element)
		{
			var meshInputs = this.ParseInputs(element);
			
			int[] p = new int[meshInputs.Count * meshInputs.Stride * 3];
			this.ParseIntArray(element.ElementValue(this.schema.pName), p);

			var subMesh = dstMesh.CreateSubmesh();
			subMesh.Material = skinAndMaterials.GetAnyMaterialFor(element.AttributeValue(this.schema.materialAttributeName));
			subMesh.VertexSourceType = VertexSourceType.TriangleList;

			var streamList = StreamListOrderedByOffset(meshInputs, subMesh);

			for (int polygonIndex = 0; polygonIndex < meshInputs.Count; ++polygonIndex)
			{
				AddVertex(p, polygonIndex*3, streamList, meshInputs.Stride);
				AddVertex(p, polygonIndex * 3+1, streamList, meshInputs.Stride);
				AddVertex(p, polygonIndex * 3+2, streamList, meshInputs.Stride);
			}
		}

		#endregion
	}
}
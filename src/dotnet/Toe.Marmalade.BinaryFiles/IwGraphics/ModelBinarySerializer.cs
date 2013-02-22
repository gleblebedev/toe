using System;
using System.Drawing;

using Autofac;

using OpenTK;

using Toe.Marmalade.IwGraphics;
using Toe.Resources;
using Toe.Utils;

namespace Toe.Marmalade.BinaryFiles.IwGraphics
{
	public class ModelBinarySerializer : IBinarySerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public ModelBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var model = this.context.Resolve<Model>();
			model.NameHash = parser.ConsumeUInt32();
			model.Flags = parser.ConsumeUInt32();
			var numVerts = parser.ConsumeUInt32();
			var numVertsUnique = parser.ConsumeUInt32();
			model.Center = parser.ConsumeVector3();
			model.Radius = parser.ConsumeFloat();
			var streamMesh = this.context.Resolve<Mesh>();
			model.Meshes.Add(streamMesh);

			streamMesh.NameHash = parser.ConsumeUInt32();
			//parser.Expect((uint)0x466dbf2a);

			var num = parser.ConsumeUInt32();
			for (; num > 0; --num)
			{
				var type = parser.ConsumeUInt32();

				var name = parser.ConsumeUInt32();
				var size = parser.ConsumeUInt32();
				var numItems = parser.ConsumeUInt32();
				var flags = parser.ConsumeUInt16();

				if (type == Hash.Get("CIwModelBlockGLUVs"))
				{
					this.ParseModelBlockGLUVs(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockCols"))
				{
					this.ParseModelBlockCols(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockGLTriList"))
				{
					this.ParseModelBlockGLTriList(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockNorms"))
				{
					this.ParseModelBlockNorms(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockVerts"))
				{
					this.ParseModelBlockVerts(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockVerts2D"))
				{
					this.ParseModelBlockVerts2D(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockBiTangents"))
				{
					this.ParseModelBlockBiTangents(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockTangents"))
				{
					this.ParseModelBlockTangents(parser, model, name, size, numItems, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelBlockChunk"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockChunkTree"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockChunkVerts"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockCols16"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockFaceFlags"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockGLPrimBase"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockGLRenderEdges"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockGLRenderVerts"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockGLTriStrip"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockGLUVs2"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockIndGroups"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimBase"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimF3"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimF4"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimFT3"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimFT4"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimG3"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimG4"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimGen3"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimGen4"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimGT3"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockPrimGT4"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockRenderEdges"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockRenderVerts"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelBlockSWOptim1"))
				{
					throw new NotImplementedException();
				}

				throw new FormatException("Unknown element");
			}

			num = parser.ConsumeUInt32();
			for (; num > 0; --num)
			{
				var type = parser.ConsumeUInt32();
				var name = parser.ConsumeUInt32();
				var flags = parser.ConsumeUInt32();

				if (type == Hash.Get("CIwModelExtPos"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelExtSelSet"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelExtSelSetEdge"))
				{
					throw new NotImplementedException();
				}
				if (type == Hash.Get("CIwModelExtSelSetFace"))
				{
					this.ParseModelExtSelSetFace(parser, model, name, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelExtSelSetVert"))
				{
					this.ParseModelExtSelSetVert(parser, model, name, flags);
					continue;
				}
				if (type == Hash.Get("CIwModelExtSphere"))
				{
					throw new NotImplementedException();
				}

				throw new FormatException("Unknown element");
			}

			num = parser.ConsumeUInt32();
			uint[] materials = new uint[num];
			for (uint matIndex = 0; matIndex < num; ++matIndex)
			{
				materials[matIndex] = parser.ConsumeUInt32();
			}

			foreach (var mesh in model.Meshes)
			{
				foreach (Surface submesh in mesh.Surfaces)
				{
					submesh.Material.HashReference = materials[submesh.Material.HashReference];
				}
			}

			return model;
		}

		#endregion

		#region Methods

		private void ParseModelBlockBiTangents(
			BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = (model.Meshes[0]);
			streamMesh.BiTangents.Clear();
			int num = (int)numItems;
			streamMesh.BiTangents.Capacity = num;
			for (int i = 0; i < num; ++i)
			{
				streamMesh.BiTangents.Add(parser.ConsumeVector3());
			}
		}

		private void ParseModelBlockCols(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = (model.Meshes[0]);
			int num = (int)numItems;
			streamMesh.Colors.Clear();
			streamMesh.Colors.Capacity = num;
			if (parser.ConsumeBool())
			{
				for (int i = 0; i < num; ++i)
				{
					byte b = parser.ConsumeByte();
					streamMesh.Colors.Add(Color.FromArgb(255, b, b, b));
				}
			}
			else
			{
				var c = parser.ConsumeByteArray(num * 4);
				for (int i = 0; i < num; ++i)
				{
					streamMesh.Colors.Add(Color.FromArgb(c[i + num * 3], c[i + num * 0], c[i + num * 1], c[i + num * 2]));
				}
			}
		}

		private void ParseModelBlockGLTriList(
			BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = (model.Meshes[0]);
			var streamSubmesh = this.context.Resolve<ModelBlockGLTriList>();
			streamSubmesh.Mesh = streamMesh;
			streamMesh.Surfaces.Add(streamSubmesh);

			streamSubmesh.Material.HashReference = parser.ConsumeUInt32();

			var indices = parser.ConsumeUInt16Array((int)numItems);
			for (int i = 0; i < indices.Length;)
			{
				UInt16 a = indices[i++];
				UInt16 b = indices[i++];
				UInt16 c = indices[i++];
				streamSubmesh.Indices.Add(a);
				streamSubmesh.Indices.Add(b);
				streamSubmesh.Indices.Add(c);
			}
			//if (serialise.IsReading())
			//{
			//    this.m_TupleIDs = new ushort[this.m_NumTupleIDs];
			//    this.prims = new _IwModelPrim[this.numItems];
			//}

			//serialise.Serialise(ref this.m_TupleIDs);

			//for (int i = 0; i < this.numItems; ++i)
			//{
			//    this.prims[i].Serialise(serialise);
			//}
		}

		private void ParseModelBlockGLUVs(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = (model.Meshes[0]);
			int num = (int)numItems;
			streamMesh.UV0.Clear();
			streamMesh.UV0.Capacity = num;
			for (int i = 0; i < num; ++i)
			{
				streamMesh.UV0.Add(parser.ConsumeVector2());
			}
		}

		private void ParseModelBlockNorms(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = (model.Meshes[0]);
			streamMesh.Normals.Clear();
			int num = (int)numItems;
			streamMesh.Normals.Capacity = num;
			for (int i = 0; i < num; ++i)
			{
				streamMesh.Normals.Add(parser.ConsumeVector3());
			}
		}

		private void ParseModelBlockTangents(
			BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = (model.Meshes[0]);
			streamMesh.Tangents.Clear();
			int num = (int)numItems;
			streamMesh.Tangents.Capacity = num;
			for (int i = 0; i < num; ++i)
			{
				streamMesh.Tangents.Add(parser.ConsumeVector3());
			}
		}

		private void ParseModelBlockVerts(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var uniqueValues = parser.ConsumeUInt16();
			var streamMesh = (model.Meshes[0]);
			streamMesh.Vertices.Clear();
			var itemsToRead = (int)uniqueValues;
			streamMesh.Vertices.EnsureAt((int)numItems - 1);
			for (int i = 0; i < itemsToRead; ++i)
			{
				float x = parser.ConsumeFloat();
				parser.Skip(1);
				streamMesh.Vertices[i] = new Vector3(x, 0, 0);
			}
			for (int i = 0; i < itemsToRead; ++i)
			{
				float x = parser.ConsumeFloat();
				parser.Skip(1);
				streamMesh.Vertices[i] = new Vector3(streamMesh.Vertices[i].X, x, 0);
			}
			for (int i = 0; i < itemsToRead; ++i)
			{
				float x = parser.ConsumeFloat();
				parser.Skip(1);
				streamMesh.Vertices[i] = new Vector3(streamMesh.Vertices[i].X, streamMesh.Vertices[i].Y, x);
			}
			while (itemsToRead < numItems)
			{
				streamMesh.Vertices[itemsToRead] = streamMesh.Vertices[parser.ConsumeUInt16()];
				++itemsToRead;
			}

			//var len = this.uniqueValues; // this.numItems; //

			//this.Resize(this.numItems);

			//if (serialise.IsWriting())
			//{
			//    throw new NotImplementedException();
			//}

			//for (int i = 0; i < len; ++i)
			//{
			//    short x = (short)(this.verts[i].X - mediane);
			//    serialise.Int16(ref x);
			//    this.verts[i].X = x + mediane;
			//}

			//for (int i = 0; i < len; ++i)
			//{
			//    short y = (short)(this.verts[i].Y - mediane);
			//    serialise.Int16(ref y);
			//    this.verts[i].Y = y + mediane;
			//}

			//for (int i = 0; i < len; ++i)
			//{
			//    short z = (short)(this.verts[i].Z - mediane);
			//    serialise.Int16(ref z);
			//    this.verts[i].Z = z + mediane;
			//}

			//ushort[] links = new ushort[this.numItems - this.uniqueValues];
			//serialise.Serialise(ref links);
			//for (int i = this.uniqueValues; i < this.numItems; ++i)
			//{
			//    this.verts[i] = this.verts[links[i - this.uniqueValues]];
			//}
		}

		private void ParseModelBlockVerts2D(
			BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			throw new NotImplementedException("Can't read ModelBlockVerts2D");
		}

		private void ParseModelExtSelSetFace(BinaryParser parser, Model model, uint name, uint flags)
		{
			var m_Flags = parser.ConsumeByte();
			var m_FlagsSW = parser.ConsumeByte();
			var m_FlagsHW = parser.ConsumeByte();
			var m_OTZOfsSW = parser.ConsumeSByte();
			var m_NumFaces = parser.ConsumeUInt32();
			var m_FaceIDs = parser.ConsumeUInt16Array((int)m_NumFaces);
			//bool m_WorldSet;     /** True if this set is a world file only set */
		}

		private void ParseModelExtSelSetVert(BinaryParser parser, Model model, uint name, uint flags)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
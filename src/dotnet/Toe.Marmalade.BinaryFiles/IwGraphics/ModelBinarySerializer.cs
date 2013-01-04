using System;
using System.Drawing;
using System.Globalization;

using Autofac;

using OpenTK;

using Toe.Marmalade.IwGx;
using Toe.Resources;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwGraphics;
using Toe.Utils.Mesh;

namespace Toe.Marmalade.BinaryFiles.IwGraphics
{
	public class ModelBinarySerializer:IBinarySerializer
	{
		private readonly IComponentContext context;

		public ModelBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var model = context.Resolve<Model>();
			model.NameHash = parser.ConsumeUInt32();
			model.Flags = parser.ConsumeUInt32();
			var numVerts = parser.ConsumeUInt32();
			var numVertsUnique = parser.ConsumeUInt32();
			model.Center = parser.ConsumeVector3();
			model.Radius = parser.ConsumeFloat();
			var streamMesh = new StreamMesh();
			model.Meshes.Add(streamMesh);

			streamMesh.NameHash = parser.ConsumeUInt32();
			//parser.Expect((uint)0x466dbf2a);

			var num = parser.ConsumeUInt32();
			for (; num>0;--num)
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
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockChunk"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockChunkTree"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockChunkVerts"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockCols16"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockFaceFlags"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockGLPrimBase"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockGLRenderEdges"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockGLRenderVerts"))
				{
					throw new NotImplementedException();
					continue;
				}
				
				if (type == Hash.Get("CIwModelBlockGLTriStrip"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockGLUVs2"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockIndGroups"))
				{
					throw new NotImplementedException();
					continue;
				}
				
				if (type == Hash.Get("CIwModelBlockPrimBase"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimF3"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimF4"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimFT3"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimFT4"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimG3"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimG4"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimGen3"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimGen4"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimGT3"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockPrimGT4"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockRenderEdges"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockRenderVerts"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockSWOptim1"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockTangents"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelBlockTangents"))
				{
					throw new NotImplementedException();
					continue;
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
					continue;
				}
				if (type == Hash.Get("CIwModelExtSelSet"))
				{
					throw new NotImplementedException();
					continue;
				}
				if (type == Hash.Get("CIwModelExtSelSetEdge"))
				{
					throw new NotImplementedException();
					continue;
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
					continue;
				}
				
				throw new FormatException("Unknown element");
			}

			num = parser.ConsumeUInt32();
			uint[] materials = new uint[num];
			for (uint matIndex=0;matIndex<num; ++matIndex)
			{
				materials[matIndex] = parser.ConsumeUInt32();
			}

			foreach (var mesh in model.Meshes)
			{
				foreach (var submesh in mesh.Submeshes)
				{
					submesh.MaterialHash = materials[submesh.MaterialHash];
				}
			}

			return model;
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

		private void ParseModelBlockNorms(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = ((StreamMesh)model.Meshes[0]);
			streamMesh.Normals.Clear();
			int num = (int)numItems;
			streamMesh.Normals.Capacity = num;
			for (int i = 0; i < num; ++i)
			{
				streamMesh.Normals.Add(parser.ConsumeVector3());
			}
		}

		private void ParseModelBlockGLUVs(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = ((StreamMesh)model.Meshes[0]);
			while (streamMesh.UV.Count < 1)
			{
				streamMesh.UV.Add(new MeshStream<Vector2>());
			}
			int num = (int)numItems;
			streamMesh.UV[0].Clear();
			streamMesh.UV[0].Capacity = num;
			for (int i = 0; i < num; ++i)
			{
				streamMesh.UV[0].Add(parser.ConsumeVector2());
			}
		}

		private void ParseModelBlockGLTriList(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = ((StreamMesh)model.Meshes[0]);
			var streamSubmesh = new StreamSubmesh(streamMesh);
			streamMesh.Submeshes.Add(streamSubmesh);

			streamSubmesh.MaterialHash = parser.ConsumeUInt32();

			var indices = parser.ConsumeUInt16Array((int)numItems);
			for (int i = 0; i < indices.Length; )
			{
				var tiangle = new StreamSubmeshTriangle();
				UInt16 index = indices[i++];
				tiangle.C = new StreamSubmeshTriangleIndexes { Vertex = index, Normal = index, Color = index, UV0 = index };
				index = indices[i++];
				tiangle.B = new StreamSubmeshTriangleIndexes { Vertex = index, Normal = index, Color = index, UV0 = index };
				index = indices[i++];
				tiangle.A = new StreamSubmeshTriangleIndexes { Vertex = index, Normal = index, Color = index, UV0 = index };
				streamSubmesh.Tris.Add(tiangle);
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

		private void ParseModelBlockCols(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var streamMesh = ((StreamMesh)model.Meshes[0]);
			int num = (int)numItems;
			streamMesh.Colors.Clear();
			streamMesh.Colors.Capacity = num;
			if (parser.ConsumeBool())
			{
				for (int i = 0; i < num; ++i)
				{
					byte b = parser.ConsumeByte();
					streamMesh.Colors.Add(Color.FromArgb(255,b,b,b));
				}
			}
			else
			{
				var c = parser.ConsumeByteArray(num * 4);
				for (int i = 0; i < num; ++i)
				{
					streamMesh.Colors.Add(Color.FromArgb(c[i+num*3],c[i+num*0],c[i+num*1],c[i+num*2]));
				}
			
			}
		}

		private void ParseModelBlockVerts2D(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			throw new NotImplementedException("Can't read ModelBlockVerts2D");
		}

		private void ParseModelBlockVerts(BinaryParser parser, Model model, uint name, uint size, uint numItems, ushort flags)
		{
			var uniqueValues = parser.ConsumeUInt16();
			var streamMesh = ((StreamMesh)model.Meshes[0]);
			streamMesh.Vertices.Clear();
			var itemsToRead = (int)uniqueValues;
			streamMesh.Vertices.EnsureAt((int)numItems - 1);
			for (int i = 0; i < itemsToRead;++i)
			{
				float x = parser.ConsumeFloat(); parser.Skip(1);
				streamMesh.Vertices[i] = new Vector3(x,0,0);
			}
			for (int i = 0; i < itemsToRead; ++i)
			{
				float x = parser.ConsumeFloat(); parser.Skip(1);
				streamMesh.Vertices[i] = new Vector3(streamMesh.Vertices[i].X, x, 0);
			}
			for (int i = 0; i < itemsToRead; ++i)
			{
				float x = parser.ConsumeFloat(); parser.Skip(1);
				streamMesh.Vertices[i] = new Vector3(streamMesh.Vertices[i].X, streamMesh.Vertices[i].Y, x);
			}
			while (itemsToRead<numItems)
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
	}
}

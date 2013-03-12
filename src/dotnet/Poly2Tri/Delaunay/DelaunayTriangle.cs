/* Poly2Tri
 * Copyright (c) 2009-2010, Poly2Tri Contributors
 * http://code.google.com/p/poly2tri/
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * * Neither the name of Poly2Tri nor the names of its contributors may be
 *   used to endorse or promote products derived from this software without specific
 *   prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Poly2Tri.Delaunay.Sweep;
using Poly2Tri.Utility;

using TriangulationPoint = OpenTK.Vector2;

namespace Poly2Tri.Delaunay
{
	public enum TriangulationMode
	{
		Unconstrained,

		Constrained,

		Polygon
	}

	public class DelaunayTriangle
	{
		#region Constants and Fields

		public FixedBitArray3 EdgeIsConstrained, EdgeIsDelaunay;

		public FixedArray3<DelaunayTriangle> Neighbors;

		public FixedArray3<TriangulationPoint> Points;

		#endregion

		#region Constructors and Destructors

		public DelaunayTriangle(TriangulationPoint p1, TriangulationPoint p2, TriangulationPoint p3)
		{
			this.Points[0] = p1;
			this.Points[1] = p2;
			this.Points[2] = p3;
		}

		#endregion

		#region Public Properties

		public bool IsInterior { get; set; }

		#endregion

		#region Public Methods and Operators

		public double Area()
		{
			double b = this.Points[0].X - this.Points[1].X;
			double h = this.Points[2].Y - this.Points[1].Y;

			return Math.Abs((b * h * 0.5f));
		}

		public TriangulationPoint Centroid()
		{
			var cx = (this.Points[0].X + this.Points[1].X + this.Points[2].X) / 3f;
			var cy = (this.Points[0].Y + this.Points[1].Y + this.Points[2].Y) / 3f;
			return new TriangulationPoint(cx, cy);
		}

		public bool Contains(TriangulationPoint p)
		{
			return this.Points.Contains(p);
		}

		/// <summary>
		/// Get the index of the neighbor that shares this edge (or -1 if it isn't shared)
		/// </summary>
		/// <returns>index of the shared edge or -1 if edge isn't shared</returns>
		public int EdgeIndex(TriangulationPoint p1, TriangulationPoint p2)
		{
			int i1 = this.Points.IndexOf(p1);
			int i2 = this.Points.IndexOf(p2);

			// Points of this triangle in the edge p1-p2
			bool a = (i1 == 0 || i2 == 0);
			bool b = (i1 == 1 || i2 == 1);
			bool c = (i1 == 2 || i2 == 2);

			if (b && c)
			{
				return 0;
			}
			if (a && c)
			{
				return 1;
			}
			if (a && b)
			{
				return 2;
			}
			return -1;
		}

		public bool GetConstrainedEdgeAcross(TriangulationPoint p)
		{
			return this.EdgeIsConstrained[this.IndexOf(p)];
		}

		public bool GetConstrainedEdgeCCW(TriangulationPoint p)
		{
			return this.EdgeIsConstrained[(this.IndexOf(p) + 2) % 3];
		}

		public bool GetConstrainedEdgeCW(TriangulationPoint p)
		{
			return this.EdgeIsConstrained[(this.IndexOf(p) + 1) % 3];
		}

		public bool GetDelaunayEdgeAcross(TriangulationPoint p)
		{
			return this.EdgeIsDelaunay[this.IndexOf(p)];
		}

		public bool GetDelaunayEdgeCCW(TriangulationPoint p)
		{
			return this.EdgeIsDelaunay[(this.IndexOf(p) + 2) % 3];
		}

		public bool GetDelaunayEdgeCW(TriangulationPoint p)
		{
			return this.EdgeIsDelaunay[(this.IndexOf(p) + 1) % 3];
		}

		public int IndexCCWFrom(TriangulationPoint p)
		{
			return (this.IndexOf(p) + 1) % 3;
		}

		public int IndexCWFrom(TriangulationPoint p)
		{
			return (this.IndexOf(p) + 2) % 3;
		}

		public int IndexOf(TriangulationPoint p)
		{
			int i = this.Points.IndexOf(p);
			if (i == -1)
			{
				throw new Exception("Calling index with a point that doesn't exist in triangle");
			}
			return i;
		}

		/// <summary>
		/// Legalize triangle by rotating clockwise around oPoint
		/// </summary>
		/// <param name="oPoint">The origin point to rotate around</param>
		/// <param name="nPoint">???</param>
		public void Legalize(TriangulationPoint oPoint, TriangulationPoint nPoint)
		{
			this.RotateCW();
			this.Points[this.IndexCCWFrom(oPoint)] = nPoint;
		}

		public void MarkConstrainedEdge(int index)
		{
			this.EdgeIsConstrained[index] = true;
		}

		public void MarkConstrainedEdge(DTSweepConstraint edge)
		{
			this.MarkConstrainedEdge(edge.P, edge.Q);
		}

		/// <summary>
		/// Mark edge as constrained
		/// </summary>
		public void MarkConstrainedEdge(TriangulationPoint p, TriangulationPoint q)
		{
			int i = this.EdgeIndex(p, q);
			if (i != -1)
			{
				this.EdgeIsConstrained[i] = true;
			}
		}

		public void MarkEdge(DelaunayTriangle triangle)
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.EdgeIsConstrained[i])
				{
					triangle.MarkConstrainedEdge(this.Points[(i + 1) % 3], this.Points[(i + 2) % 3]);
				}
			}
		}

		public void MarkEdge(List<DelaunayTriangle> tList)
		{
			foreach (DelaunayTriangle t in tList)
			{
				for (int i = 0; i < 3; i++)
				{
					if (t.EdgeIsConstrained[i])
					{
						this.MarkConstrainedEdge(t.Points[(i + 1) % 3], t.Points[(i + 2) % 3]);
					}
				}
			}
		}

		/// <summary>
		/// Exhaustive search to update neighbor pointers
		/// </summary>
		public void MarkNeighbor(DelaunayTriangle t)
		{
			// Points of this triangle also belonging to t
			bool a = t.Contains(this.Points[0]);
			bool b = t.Contains(this.Points[1]);
			bool c = t.Contains(this.Points[2]);

			if (b && c)
			{
				this.Neighbors[0] = t;
				t.MarkNeighbor(this.Points[1], this.Points[2], this);
			}
			else if (a && c)
			{
				this.Neighbors[1] = t;
				t.MarkNeighbor(this.Points[0], this.Points[2], this);
			}
			else if (a && b)
			{
				this.Neighbors[2] = t;
				t.MarkNeighbor(this.Points[0], this.Points[1], this);
			}
			else
			{
				throw new Exception("Failed to mark neighbor, doesn't share an edge!");
			}
		}

		/// <summary>
		/// Finalize edge marking
		/// </summary>
		public void MarkNeighborEdges()
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.EdgeIsConstrained[i] && this.Neighbors[i] != null)
				{
					this.Neighbors[i].MarkConstrainedEdge(this.Points[(i + 1) % 3], this.Points[(i + 2) % 3]);
				}
			}
		}

		public DelaunayTriangle NeighborAcrossFrom(TriangulationPoint point)
		{
			return this.Neighbors[this.Points.IndexOf(point)];
		}

		public DelaunayTriangle NeighborCCWFrom(TriangulationPoint point)
		{
			return this.Neighbors[(this.Points.IndexOf(point) + 2) % 3];
		}

		public DelaunayTriangle NeighborCWFrom(TriangulationPoint point)
		{
			return this.Neighbors[(this.Points.IndexOf(point) + 1) % 3];
		}

		/// <param name="t">Opposite triangle</param>
		/// <param name="p">The point in t that isn't shared between the triangles</param>
		public TriangulationPoint OppositePoint(DelaunayTriangle t, TriangulationPoint p)
		{
			Debug.Assert(t != this, "self-pointer error");
			return this.PointCWFrom(t.PointCWFrom(p));
		}

		public TriangulationPoint PointCCWFrom(TriangulationPoint point)
		{
			return this.Points[(this.IndexOf(point) + 1) % 3];
		}

		public TriangulationPoint PointCWFrom(TriangulationPoint point)
		{
			return this.Points[(this.IndexOf(point) + 2) % 3];
		}

		public void SetConstrainedEdgeAcross(TriangulationPoint p, bool ce)
		{
			this.EdgeIsConstrained[this.IndexOf(p)] = ce;
		}

		public void SetConstrainedEdgeCCW(TriangulationPoint p, bool ce)
		{
			this.EdgeIsConstrained[(this.IndexOf(p) + 2) % 3] = ce;
		}

		public void SetConstrainedEdgeCW(TriangulationPoint p, bool ce)
		{
			this.EdgeIsConstrained[(this.IndexOf(p) + 1) % 3] = ce;
		}

		public void SetDelaunayEdgeAcross(TriangulationPoint p, bool ce)
		{
			this.EdgeIsDelaunay[this.IndexOf(p)] = ce;
		}

		public void SetDelaunayEdgeCCW(TriangulationPoint p, bool ce)
		{
			this.EdgeIsDelaunay[(this.IndexOf(p) + 2) % 3] = ce;
		}

		public void SetDelaunayEdgeCW(TriangulationPoint p, bool ce)
		{
			this.EdgeIsDelaunay[(this.IndexOf(p) + 1) % 3] = ce;
		}

		public override string ToString()
		{
			return this.Points[0] + "," + this.Points[1] + "," + this.Points[2];
		}

		#endregion

		#region Methods

		/// <summary>
		/// Update neighbor pointers
		/// </summary>
		/// <param name="p1">Point 1 of the shared edge</param>
		/// <param name="p2">Point 2 of the shared edge</param>
		/// <param name="t">This triangle's new neighbor</param>
		private void MarkNeighbor(TriangulationPoint p1, TriangulationPoint p2, DelaunayTriangle t)
		{
			int i = this.EdgeIndex(p1, p2);
			if (i == -1)
			{
				throw new Exception("Error marking neighbors -- t doesn't contain edge p1-p2!");
			}
			this.Neighbors[i] = t;
		}

		private void RotateCW()
		{
			var t = this.Points[2];
			this.Points[2] = this.Points[1];
			this.Points[1] = this.Points[0];
			this.Points[0] = t;
		}

		#endregion
	}
}
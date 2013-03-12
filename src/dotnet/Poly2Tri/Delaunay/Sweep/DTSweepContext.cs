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

namespace Poly2Tri.Delaunay.Sweep
{
	public class DTSweepContext : TriangulationContext
	{
		// Inital triangle factor, seed triangle will extend 30% of 
		// PointSet width to both left and right.

		#region Constants and Fields

		public DTSweepBasin Basin = new DTSweepBasin();

		public DTSweepEdgeEvent EdgeEvent = new DTSweepEdgeEvent();

		public AdvancingFront Front;

		private readonly float ALPHA = 0.3f;

		private readonly DTSweepPointComparator _comparator = new DTSweepPointComparator();

		#endregion

		#region Constructors and Destructors

		public DTSweepContext()
		{
			this.Clear();
		}

		#endregion

		#region Public Properties

		public override TriangulationAlgorithm Algorithm
		{
			get
			{
				return TriangulationAlgorithm.DTSweep;
			}
		}

		public TriangulationPoint Head { get; set; }

		public override bool IsDebugEnabled
		{
			get
			{
				return base.IsDebugEnabled;
			}
			protected set
			{
				if (value && this.DebugContext == null)
				{
					this.DebugContext = new DTSweepDebugContext(this);
				}
				base.IsDebugEnabled = value;
			}
		}

		public TriangulationPoint Tail { get; set; }

		#endregion

		#region Public Methods and Operators

		public void AddNode(AdvancingFrontNode node)
		{
			//        Console.WriteLine( "add:" + node.key + ":" + System.identityHashCode(node.key));
			//        m_nodeTree.put( node.getKey(), node );
			this.Front.AddNode(node);
		}

		public override void Clear()
		{
			base.Clear();
			this.Triangles.Clear();
		}

		public void CreateAdvancingFront()
		{
			AdvancingFrontNode head, tail, middle;
			// Initial triangle
			DelaunayTriangle iTriangle = new DelaunayTriangle(this.Points[0], this.Tail, this.Head);
			this.Triangles.Add(iTriangle);

			head = new AdvancingFrontNode(iTriangle.Points[1]);
			head.Triangle = iTriangle;
			middle = new AdvancingFrontNode(iTriangle.Points[0]);
			middle.Triangle = iTriangle;
			tail = new AdvancingFrontNode(iTriangle.Points[2]);

			this.Front = new AdvancingFront(head, tail);
			this.Front.AddNode(middle);

			// TODO: I think it would be more intuitive if head is middles next and not previous
			//       so swap head and tail
			this.Front.Head.Next = middle;
			middle.Next = this.Front.Tail;
			middle.Prev = this.Front.Head;
			this.Front.Tail.Prev = middle;
		}

		public void FinalizeTriangulation()
		{
			this.Triangulatable.AddTriangles(this.Triangles);
			this.Triangles.Clear();
		}

		public AdvancingFrontNode LocateNode(TriangulationPoint point)
		{
			return this.Front.LocateNode(point);
		}

		/// <summary>
		/// Try to map a node to all sides of this triangle that don't have 
		/// a neighbor.
		/// </summary>
		public void MapTriangleToNodes(DelaunayTriangle t)
		{
			for (int i = 0; i < 3; i++)
			{
				if (t.Neighbors[i] == null)
				{
					AdvancingFrontNode n = this.Front.LocatePoint(t.PointCWFrom(t.Points[i]));
					if (n != null)
					{
						n.Triangle = t;
					}
				}
			}
		}

		public void MeshClean(DelaunayTriangle triangle)
		{
			this.MeshCleanReq(triangle);
		}

		public override TriangulationConstraint NewConstraint(TriangulationPoint a, TriangulationPoint b)
		{
			return new DTSweepConstraint(a, b);
		}

		public override void PrepareTriangulation(Triangulatable t)
		{
			base.PrepareTriangulation(t);

			double xmax, xmin;
			double ymax, ymin;

			xmax = xmin = this.Points[0].X;
			ymax = ymin = this.Points[0].Y;

			// Calculate bounds. Should be combined with the sorting
			foreach (TriangulationPoint p in this.Points)
			{
				if (p.X > xmax)
				{
					xmax = p.X;
				}
				if (p.X < xmin)
				{
					xmin = p.X;
				}
				if (p.Y > ymax)
				{
					ymax = p.Y;
				}
				if (p.Y < ymin)
				{
					ymin = p.Y;
				}
			}

			double deltaX = this.ALPHA * (xmax - xmin);
			double deltaY = this.ALPHA * (ymax - ymin);
			TriangulationPoint p1 = new TriangulationPoint(xmax + deltaX, ymin - deltaY);
			TriangulationPoint p2 = new TriangulationPoint(xmin - deltaX, ymin - deltaY);

			this.Head = p1;
			this.Tail = p2;

			//        long time = System.nanoTime();
			// Sort the points along y-axis
			this.Points.Sort(this._comparator);
			//        logger.info( "Triangulation setup [{}ms]", ( System.nanoTime() - time ) / 1e6 );
		}

		public void RemoveFromList(DelaunayTriangle triangle)
		{
			this.Triangles.Remove(triangle);
			// TODO: remove all neighbor pointers to this triangle
			//        for( int i=0; i<3; i++ )
			//        {
			//            if( triangle.neighbors[i] != null )
			//            {
			//                triangle.neighbors[i].clearNeighbor( triangle );
			//            }
			//        }
			//        triangle.clearNeighbors();
		}

		public void RemoveNode(AdvancingFrontNode node)
		{
			//        Console.WriteLine( "remove:" + node.key + ":" + System.identityHashCode(node.key));
			//        m_nodeTree.delete( node.getKey() );
			this.Front.RemoveNode(node);
		}

		#endregion

		#region Methods

		private void MeshCleanReq(DelaunayTriangle triangle)
		{
			if (triangle != null && !triangle.IsInterior)
			{
				triangle.IsInterior = true;
				this.Triangulatable.AddTriangle(triangle);

				for (int i = 0; i < 3; i++)
				{
					if (!triangle.EdgeIsConstrained[i])
					{
						this.MeshCleanReq(triangle.Neighbors[i]);
					}
				}
			}
		}

		#endregion
	}
}
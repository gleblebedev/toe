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
using System.Text;

namespace Poly2Tri.Delaunay.Sweep
{
	public class DTSweepConstraint : TriangulationConstraint
	{
		#region Constructors and Destructors

		/// <summary>
		/// Give two points in any order. Will always be ordered so
		/// that q.y > p.y and q.x > p.x if same y value 
		/// </summary>
		public DTSweepConstraint(TriangulationPoint p1, TriangulationPoint p2)
		{
			this.P = p1;
			this.Q = p2;
			if (p1.Y > p2.Y)
			{
				this.Q = p1;
				this.P = p2;
			}
			else if (p1.Y == p2.Y)
			{
				if (p1.X > p2.X)
				{
					this.Q = p1;
					this.P = p2;
				}
				else if (p1.X == p2.X)
				{
					//                logger.info( "Failed to create constraint {}={}", p1, p2 );
					//                throw new DuplicatePointException( p1 + "=" + p2 );
					//                return;
				}
			}
			this.Q.AddEdge(this);
		}

		#endregion
	}

	public class DTSweepDebugContext : TriangulationDebugContext
	{
		/*
		 * Fields used for visual representation of current triangulation
		 */

		#region Constants and Fields

		private DTSweepConstraint _activeConstraint;

		private AdvancingFrontNode _activeNode;

		private TriangulationPoint _activePoint;

		private DelaunayTriangle _primaryTriangle;

		private DelaunayTriangle _secondaryTriangle;

		#endregion

		#region Constructors and Destructors

		public DTSweepDebugContext(DTSweepContext tcx)
			: base(tcx)
		{
		}

		#endregion

		#region Public Properties

		public DTSweepConstraint ActiveConstraint
		{
			get
			{
				return this._activeConstraint;
			}
			set
			{
				this._activeConstraint = value;
				this._tcx.Update("set ActiveConstraint");
			}
		}

		public AdvancingFrontNode ActiveNode
		{
			get
			{
				return this._activeNode;
			}
			set
			{
				this._activeNode = value;
				this._tcx.Update("set ActiveNode");
			}
		}

		public TriangulationPoint ActivePoint
		{
			get
			{
				return this._activePoint;
			}
			set
			{
				this._activePoint = value;
				this._tcx.Update("set ActivePoint");
			}
		}

		public bool IsDebugContext
		{
			get
			{
				return true;
			}
		}

		public DelaunayTriangle PrimaryTriangle
		{
			get
			{
				return this._primaryTriangle;
			}
			set
			{
				this._primaryTriangle = value;
				this._tcx.Update("set PrimaryTriangle");
			}
		}

		public DelaunayTriangle SecondaryTriangle
		{
			get
			{
				return this._secondaryTriangle;
			}
			set
			{
				this._secondaryTriangle = value;
				this._tcx.Update("set SecondaryTriangle");
			}
		}

		#endregion

		#region Public Methods and Operators

		public override void Clear()
		{
			this.PrimaryTriangle = null;
			this.SecondaryTriangle = null;
			this.ActivePoint = null;
			this.ActiveNode = null;
			this.ActiveConstraint = null;
		}

		#endregion
	}

	public class AdvancingFront
	{
		#region Constants and Fields

		public AdvancingFrontNode Head;

		public AdvancingFrontNode Tail;

		protected AdvancingFrontNode Search;

		#endregion

		#region Constructors and Destructors

		public AdvancingFront(AdvancingFrontNode head, AdvancingFrontNode tail)
		{
			this.Head = head;
			this.Tail = tail;
			this.Search = head;
			this.AddNode(head);
			this.AddNode(tail);
		}

		#endregion

		#region Public Methods and Operators

		public void AddNode(AdvancingFrontNode node)
		{
		}

		/// <summary>
		/// We use a balancing tree to locate a node smaller or equal to given key value (in theory)
		/// </summary>
		public AdvancingFrontNode LocateNode(TriangulationPoint point)
		{
			return LocateNode(point.X);
		}

		/// <summary>
		/// This implementation will use simple node traversal algorithm to find a point on the front
		/// </summary>
		public AdvancingFrontNode LocatePoint(TriangulationPoint point)
		{
			double px = point.X;
			AdvancingFrontNode node = this.FindSearchNode(px);
			double nx = node.Point.X;

			if (px == nx)
			{
				if (point != node.Point)
				{
					// We might have two nodes with same x value for a short time
					if (point == node.Prev.Point)
					{
						node = node.Prev;
					}
					else if (point == node.Next.Point)
					{
						node = node.Next;
					}
					else
					{
						throw new Exception("Failed to find Node for given afront point");
					}
				}
			}
			else if (px < nx)
			{
				while ((node = node.Prev) != null)
				{
					if (point == node.Point)
					{
						break;
					}
				}
			}
			else
			{
				while ((node = node.Next) != null)
				{
					if (point == node.Point)
					{
						break;
					}
				}
			}
			this.Search = node;
			return node;
		}

		public void RemoveNode(AdvancingFrontNode node)
		{
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			AdvancingFrontNode node = this.Head;
			while (node != this.Tail)
			{
				sb.Append(node.Point.X).Append("->");
				node = node.Next;
			}
			sb.Append(this.Tail.Point.X);
			return sb.ToString();
		}

		#endregion

		#region Methods

		/// <summary>
		/// MM:  This seems to be used by LocateNode to guess a position in the implicit linked list of AdvancingFrontNodes near x
		///      Removed an overload that depended on this being exact
		/// </summary>
		private AdvancingFrontNode FindSearchNode(double x)
		{
			return this.Search;
		}

		private AdvancingFrontNode LocateNode(double x)
		{
			AdvancingFrontNode node = this.FindSearchNode(x);
			if (x < node.Value)
			{
				while ((node = node.Prev) != null)
				{
					if (x >= node.Value)
					{
						this.Search = node;
						return node;
					}
				}
			}
			else
			{
				while ((node = node.Next) != null)
				{
					if (x < node.Value)
					{
						this.Search = node.Prev;
						return node.Prev;
					}
				}
			}
			return null;
		}

		#endregion
	}

	public class AdvancingFrontNode
	{
		#region Constants and Fields

		public AdvancingFrontNode Next;

		public TriangulationPoint Point;

		public AdvancingFrontNode Prev;

		public DelaunayTriangle Triangle;

		public double Value;

		#endregion

		#region Constructors and Destructors

		public AdvancingFrontNode(TriangulationPoint point)
		{
			this.Point = point;
			this.Value = point.X;
		}

		#endregion

		#region Public Properties

		public bool HasNext
		{
			get
			{
				return this.Next != null;
			}
		}

		public bool HasPrev
		{
			get
			{
				return this.Prev != null;
			}
		}

		#endregion
	}
}
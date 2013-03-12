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

using System.Collections.Generic;

using Poly2Tri.Delaunay;
using Poly2Tri.Delaunay.Sweep;

namespace Poly2Tri
{
	public abstract class TriangulationContext
	{
		#region Constants and Fields

		public readonly List<TriangulationPoint> Points = new List<TriangulationPoint>(200);

		public readonly List<DelaunayTriangle> Triangles = new List<DelaunayTriangle>();

		#endregion

		#region Public Properties

		public abstract TriangulationAlgorithm Algorithm { get; }

		public DTSweepDebugContext DTDebugContext
		{
			get
			{
				return this.DebugContext as DTSweepDebugContext;
			}
		}

		public TriangulationDebugContext DebugContext { get; protected set; }

		public virtual bool IsDebugEnabled { get; protected set; }

		public int StepCount { get; private set; }

		public Triangulatable Triangulatable { get; private set; }

		public TriangulationMode TriangulationMode { get; protected set; }

		#endregion

		#region Public Methods and Operators

		public virtual void Clear()
		{
			this.Points.Clear();
			if (this.DebugContext != null)
			{
				this.DebugContext.Clear();
			}
			this.StepCount = 0;
		}

		public void Done()
		{
			this.StepCount++;
		}

		public abstract TriangulationConstraint NewConstraint(TriangulationPoint a, TriangulationPoint b);

		public virtual void PrepareTriangulation(Triangulatable t)
		{
			this.Triangulatable = t;
			this.TriangulationMode = t.TriangulationMode;
			t.Prepare(this);
		}

		public void Update(string message)
		{
		}

		#endregion
	}
}
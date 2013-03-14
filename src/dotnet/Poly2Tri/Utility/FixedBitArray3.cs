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
using System.Collections;
using System.Collections.Generic;

namespace Poly2Tri.Utility
{
	public struct FixedBitArray3 : IEnumerable<bool>
	{
		#region Constants and Fields

		public bool _0, _1, _2;

		#endregion

		#region Public Indexers

		public bool this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this._0;
					case 1:
						return this._1;
					case 2:
						return this._2;
					default:
						throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						this._0 = value;
						break;
					case 1:
						this._1 = value;
						break;
					case 2:
						this._2 = value;
						break;
					default:
						throw new IndexOutOfRangeException();
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Clear()
		{
			this._0 = this._1 = this._2 = false;
		}

		public void Clear(bool value)
		{
			for (int i = 0; i < 3; ++i)
			{
				if (this[i] == value)
				{
					this[i] = false;
				}
			}
		}

		public bool Contains(bool value)
		{
			for (int i = 0; i < 3; ++i)
			{
				if (this[i] == value)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerator<bool> GetEnumerator()
		{
			return this.Enumerate().GetEnumerator();
		}

		public int IndexOf(bool value)
		{
			for (int i = 0; i < 3; ++i)
			{
				if (this[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		#endregion

		#region Explicit Interface Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Methods

		private IEnumerable<bool> Enumerate()
		{
			for (int i = 0; i < 3; ++i)
			{
				yield return this[i];
			}
		}

		#endregion
	}

	/*
	 * Extends the PointSet by adding some Constraints on how it will be triangulated<br>
	 * A constraint defines an edge between two points in the set, these edges can not
	 * be crossed. They will be enforced triangle edges after a triangulation.
	 * <p>
	 * 
	 * 
	 * @author Thomas �hl�n, thahlen@gmail.com
	 */
}
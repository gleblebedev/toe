using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Toe.Utils.Mesh.Bsp
{
	public class BspEntity : DynamicObject
	{
		#region Constants and Fields

		private readonly Dictionary<string, object> _members = new Dictionary<string, object>();

		#endregion

		#region Public Indexers

		public object this[string key]
		{
			get
			{
				object v;
				if (!this._members.TryGetValue(key, out v))
				{
					return null;
				}
				return v;
			}
			set
			{
				this._members[key] = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Return all dynamic member names
		/// </summary>
		/// <returns>
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._members.Keys;
		}

		/// <summary>
		/// When user accesses something, return the value if we have it
		/// </summary>      
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (this._members.ContainsKey(binder.Name))
			{
				result = this._members[binder.Name];
				return true;
			}
			else
			{
				result = null;
				return true;
				//return base.TryGetMember(binder, out result);
			}
		}

		/// <summary>
		/// If a property value is a delegate, invoke it
		/// </summary>     
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			if (this._members.ContainsKey(binder.Name) && this._members[binder.Name] is Delegate)
			{
				result = (this._members[binder.Name] as Delegate).DynamicInvoke(args);
				return true;
			}
			else
			{
				return base.TryInvokeMember(binder, args, out result);
			}
		}

		/// <summary>
		/// When a new property is set, 
		/// add the property name and value to the dictionary
		/// </summary>     
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (!this._members.ContainsKey(binder.Name))
			{
				this._members.Add(binder.Name, value);
			}
			else
			{
				this._members[binder.Name] = value;
			}

			return true;
		}

		#endregion
	}
}
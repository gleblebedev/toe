using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Messaging.Types
{
	public class TypeRegistry
	{
		#region Constants and Fields

		private readonly ITypeBinarySerializer[] typeBinarySerializers;

		#endregion

		#region Constructors and Destructors

		private TypeRegistry(IEnumerable<ITypeBinarySerializer> typeBinarySerializers)
		{
			this.typeBinarySerializers = typeBinarySerializers.OrderBy(x => x.PropertyType).ToArray();
		}

		#endregion

		#region Public Properties

		public static IEnumerable<ITypeBinarySerializer> StandartTypes
		{
			get
			{
				return new ITypeBinarySerializer[]
					{ 
						new Int32BinarySerializer(), 
						new StringBinarySerializer(), 
						new SignleBinarySerializer(),
#if WINDOWS_PHONE
						new VectorXYZBinarySerializer(), 
#endif
					};
			}
		}

		#endregion

		#region Public Methods and Operators

		public static TypeRegistry CreateDefault()
		{
			return new TypeRegistry(StandartTypes);
		}

		public ITypeBinarySerializer ResolveSerializer(int propertyType)
		{
			return this.ResolveSerializer(propertyType, 0, this.typeBinarySerializers.Length - 1);
		}

		#endregion

		#region Methods

		private ITypeBinarySerializer ResolveSerializer(int propertyType, int leftIndex, int rightIndex)
		{
		retry:
			if (leftIndex > rightIndex) return null;
			// calculate midpoint to cut set in half
			int midIndex = (leftIndex + rightIndex) >> 1;

			// three-way comparison
			if (this.typeBinarySerializers[midIndex].PropertyType > propertyType)
			{
				// key is in lower subset
				rightIndex = midIndex - 1;
				goto retry;
			}
			if (this.typeBinarySerializers[midIndex].PropertyType < propertyType)
			{
				// key is in upper subset
				leftIndex = midIndex + 1;
				goto retry;
			}
			// key has been found
			return this.typeBinarySerializers[midIndex];
		}

		#endregion
	}
}
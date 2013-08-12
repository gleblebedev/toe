using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.Messaging.Types
{
	public class TypeRegistry : Registry<ITypeBinarySerializer>
	{
		#region Constructors and Destructors

		public TypeRegistry(IEnumerable<ITypeBinarySerializer> typeBinarySerializers)
			: base(typeBinarySerializers, x => x.PropertyType)
		{
		}

		public TypeRegistry(IEnumerable<IEnumerable<ITypeBinarySerializer>> typeBinarySerializers)
			: base(Merge(typeBinarySerializers), x => x.PropertyType)
		{
		}

		#endregion

		#region Public Properties

		public static IEnumerable<ITypeBinarySerializer> StandartTypes
		{
			get
			{
				return new ITypeBinarySerializer[]
					{
						new Int32BinarySerializer(), new StringBinarySerializer(), new SignleBinarySerializer(),
						new VectorXYBinarySerializer(), new VectorXYZBinarySerializer(), new VectorXYZWBinarySerializer(),
						new QuaternionXYZWBinarySerializer(),
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
			return this.BinarySearch(propertyType, 0, this.sortedValues.Length - 1);
		}

		public bool TryResolvePropertyType(Type fieldType, out int v)
		{
			foreach (var serializer in this.sortedValues)
			{
				if (serializer.CanHandleType(fieldType))
				{
					v = serializer.PropertyType;
					return true;
				}
			}
			v = PropertyTypes.Unknown;
			return false;
		}

		#endregion

		#region Methods

		private static IEnumerable<ITypeBinarySerializer> Merge(
			IEnumerable<IEnumerable<ITypeBinarySerializer>> typeBinarySerializers)
		{
			//TODO: Replace with sort with order preservation + unique()
			Dictionary<int, ITypeBinarySerializer> map = new Dictionary<int, ITypeBinarySerializer>();
			foreach (var typeBinarySerializer in typeBinarySerializers)
			{
				foreach (var binarySerializer in typeBinarySerializer)
				{
					map[binarySerializer.PropertyType] = binarySerializer;
				}
			}
			return (from a in map select a.Value);
		}

		#endregion
	}
}
using System.Collections.Generic;

namespace Toe.Messaging.Types
{
	public class TypeRegistry : Registry<ITypeBinarySerializer>
	{
		#region Constructors and Destructors

	    public TypeRegistry(IEnumerable<ITypeBinarySerializer> typeBinarySerializers)
			: base(typeBinarySerializers, x => x.PropertyType)
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
			return this.BinarySearch(propertyType, 0, this.sortedValues.Length - 1);
		}

		#endregion
	}
}
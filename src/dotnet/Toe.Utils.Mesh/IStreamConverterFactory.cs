using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public interface IStreamConverterFactory
	{
		IList<TRes> ResolveConverter<T,TRes>(IList<T> arrayMeshStream);
	}

}
using System.Collections.Generic;

namespace Toe.Utils.Mesh
{
	public interface IMeshStream
	{
		IList<TValue> GetReader<TValue>();

		IStreamConverterFactory ConverterFactory { get; set; }
	}
	public interface IMeshStream<T> : IList<T>, IMeshStream
	{
		
	}
}
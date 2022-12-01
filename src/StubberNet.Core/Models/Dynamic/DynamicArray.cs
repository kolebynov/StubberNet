using System.Collections;

namespace StubberNet.Core.Models.Dynamic
{
	public class DynamicArray : Token, IReadOnlyCollection<Token>
	{
		private readonly IReadOnlyCollection<Token> arrayData;

		public int Count => arrayData.Count;

		public DynamicArray(IReadOnlyCollection<Token> arrayData)
		{
			this.arrayData = arrayData ?? throw new ArgumentNullException(nameof(arrayData));
		}

		public IEnumerator<Token> GetEnumerator() => arrayData.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

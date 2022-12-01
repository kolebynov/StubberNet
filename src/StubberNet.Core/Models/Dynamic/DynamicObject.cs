using System.Collections;

namespace StubberNet.Core.Models.Dynamic
{
	public class DynamicObject : Token, IEnumerable<KeyValuePair<string, Token>>
	{
		private readonly IDictionary<string, Token> objectData;

		public Token this[string property] => objectData[property];

		public DynamicObject(IDictionary<string, Token> objectData)
		{
			this.objectData = objectData ?? throw new ArgumentNullException(nameof(objectData));
		}

		public IEnumerator<KeyValuePair<string, Token>> GetEnumerator() => objectData.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

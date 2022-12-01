namespace StubberNet.Core.Models.Dynamic
{
	public class StringValue : Token
	{
		public string Value { get; }

		public StringValue(string value)
		{
			Value = value ?? throw new ArgumentNullException(nameof(value));
		}

		public override string ToString() => Value;

		public static implicit operator StringValue(string str) => new(str);
	}
}

namespace StubberNet.Core.Models.Dynamic;

public abstract class Token
{
	public static implicit operator Token(string str) => new StringValue(str);
}
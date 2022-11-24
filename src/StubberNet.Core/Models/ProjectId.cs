namespace StubberNet.Core.Models;

public readonly record struct ProjectId
{
	public string Value { get; }

	public static ProjectId Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
		}

		return new ProjectId(value);
	}

	private ProjectId(string value)
	{
		Value = value;
	}
}
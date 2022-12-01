using StubberNet.Core.Models.Dynamic;

namespace StubberNet.Core.Models.Resource;

public sealed class StaticResourceData : IResourceData
{
	private static readonly StaticResourceDefinition DefinitionInstance = new();

	private readonly DynamicArray _data;

	public IResourceDefinition Definition => DefinitionInstance;

	public IEnumerable<Token> Data => _data;

	public StaticResourceData(DynamicArray data)
	{
		_data = data ?? throw new ArgumentNullException(nameof(data));
	}
}
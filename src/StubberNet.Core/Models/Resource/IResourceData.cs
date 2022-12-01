using StubberNet.Core.Models.Dynamic;

namespace StubberNet.Core.Models.Resource;

public interface IResourceData
{
	IResourceDefinition Definition { get; }

	IEnumerable<Token> Data { get; }
}
using System.Text.Json;
using System.Text.Json.Serialization;
using StubberNet.Core.Models.Dynamic;

namespace StubberNet.Core.Infrastructure;

public sealed class TokenJsonConverter : JsonConverter<Token>
{
	private static readonly JsonConverter<DynamicArray> DefaultArrayConverter =
		(JsonConverter<DynamicArray>)JsonSerializerOptions.Default.GetConverter(typeof(DynamicArray));
	private static readonly JsonConverter<DynamicObject> DefaultObjectConverter =
		(JsonConverter<DynamicObject>)JsonSerializerOptions.Default.GetConverter(typeof(DynamicObject));

	public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(Token));

	public override Token? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		throw new NotSupportedException();

	public override void Write(Utf8JsonWriter writer, Token value, JsonSerializerOptions options)
	{
		switch (value)
		{
			case DynamicArray dynamicArray:
				DefaultArrayConverter.Write(writer, dynamicArray, options);
				break;
			case DynamicObject dynamicObject:
				DefaultObjectConverter.Write(writer, dynamicObject, options);
				break;
			case StringValue stringValue:
				writer.WriteStringValue(stringValue.Value);
				break;
			default:
				throw new JsonException($"Unsupported token type {value.GetType()}");
		}
	}
}
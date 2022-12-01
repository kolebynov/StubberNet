using System.Net;

namespace StubberNet.Core.Models.ApiEndpoint;

public sealed class Response
{
	private readonly Func<Stream, CancellationToken, Task> _responseBodyWriter;

	public HttpStatusCode StatusCode { get; }

	public IReadOnlyDictionary<string, string> Headers { get; }

	public Response(HttpStatusCode statusCode, IReadOnlyDictionary<string, string> headers,
		Func<Stream, CancellationToken, Task> responseBodyWriter)
	{
		StatusCode = statusCode;
		Headers = headers ?? throw new ArgumentNullException(nameof(headers));
		_responseBodyWriter = responseBodyWriter ?? throw new ArgumentNullException(nameof(responseBodyWriter));
	}

	public Task WriteResponseBodyAsync(Stream response, CancellationToken cancellationToken) =>
		_responseBodyWriter(response, cancellationToken);
}
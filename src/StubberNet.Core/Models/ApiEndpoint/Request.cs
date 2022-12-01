namespace StubberNet.Core.Models.ApiEndpoint;

public sealed class Request
{
	private static readonly Uri BaseUri = new("http://tempuri");

	public HttpMethod Method { get; }

	public Uri Uri { get; }

	public Request(HttpMethod method, string relativeUriString)
	{
		if (string.IsNullOrWhiteSpace(relativeUriString))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(relativeUriString));
		}

		if (!Uri.TryCreate(relativeUriString, UriKind.Relative, out var relativeUri))
		{
			throw new ArgumentException("Uri string must be a relative uri", nameof(relativeUriString));
		}

		Method = method ?? throw new ArgumentNullException(nameof(method));
		Uri = new Uri(BaseUri, relativeUri);
	}
}
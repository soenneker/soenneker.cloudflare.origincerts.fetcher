using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher;

/// <inheritdoc cref="ICloudflareOriginCertFetcher"/>
public sealed class CloudflareOriginCertFetcher : ICloudflareOriginCertFetcher
{
    private readonly IHttpClientCache _httpClientCache;

    public CloudflareOriginCertFetcher(IHttpClientCache httpClientCache)
    {
        _httpClientCache = httpClientCache;
    }

    public async ValueTask<List<string>> GetSharedAopThumbprints(CancellationToken cancellationToken = default)
    {
        HttpClient client = await _httpClientCache.Get(nameof(CloudflareOriginCertFetcher), cancellationToken: cancellationToken).NoSync();

        const string pemUrl = "https://developers.cloudflare.com/ssl/static/authenticated_origin_pull_ca.pem";
        string pem = await client.GetStringAsync(pemUrl, cancellationToken).NoSync();
        return ParsePemThumbprints(pem);
    }

    /// <summary>
    /// Parses one or more PEM certificates and returns SHA1 thumbprints.
    /// </summary>
    public static List<string> ParsePemThumbprints(string pem)
    {
        var results = new List<string>();
        var regex = new Regex("-----BEGIN CERTIFICATE-----(.*?)-----END CERTIFICATE-----", RegexOptions.Singleline);

        foreach (Match match in regex.Matches(pem))
        {
            string base64 = match.Groups[1].Value.Replace("\r", "").Replace("\n", "");
            byte[] raw = base64.ToBytesFromBase64();

            using X509Certificate2 cert = X509CertificateLoader.LoadCertificate(raw);
            results.Add(cert.Thumbprint);
        }

        return results;
    }
}
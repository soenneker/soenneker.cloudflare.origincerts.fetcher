using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Hashing.Sha256;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher;

/// <inheritdoc cref="ICloudflareOriginCertFetcher" />
public sealed class CloudflareOriginCertFetcher : ICloudflareOriginCertFetcher
{
    private static readonly Sha256HashingUtil _sha256 = new();

    private const string PemUrl = "https://developers.cloudflare.com/ssl/static/authenticated_origin_pull_ca.pem";
    private readonly IHttpClientCache _httpClientCache;

    public CloudflareOriginCertFetcher(IHttpClientCache httpClientCache)
    {
        _httpClientCache = httpClientCache;
    }

    public async ValueTask<List<string>> GetSharedAopThumbprints(CancellationToken cancellationToken = default)
    {
        HttpClient client = await _httpClientCache.Get(nameof(CloudflareOriginCertFetcher), cancellationToken: cancellationToken)
                                                  .NoSync();

        string pem = await client.GetStringAsync(PemUrl, cancellationToken)
                                 .NoSync();
        return ParsePemThumbprints(pem);
    }

    public async ValueTask<string> GetSharedAopCertificatePem(CancellationToken cancellationToken = default)
    {
        HttpClient client = await _httpClientCache.Get(nameof(CloudflareOriginCertFetcher), cancellationToken: cancellationToken)
                                                  .NoSync();

        return await client.GetStringAsync(PemUrl, cancellationToken)
                           .NoSync();
    }

    /// <summary>
    /// Parses one or more PEM certificates and returns SHA-256 thumbprints.
    /// </summary>
    public static List<string> ParsePemThumbprints(string pem)
    {
        var results = new List<string>();
        Regex regex = CertRegex.PemCertRegex();

        foreach (Match match in regex.Matches(pem))
        {
            // Base64 payload inside PEM is line-wrapped; decode helpers often do NOT accept embedded whitespace.
            string base64 = match.Groups[1]
                                 .Value.RemoveWhiteSpace();
            byte[] raw = base64.ToBytesFromBase64();

            using X509Certificate2 cert = X509CertificateLoader.LoadCertificate(raw);
            results.Add(System.Convert.ToHexString(_sha256.Hash(cert.RawData)));
        }

        if (results.Count == 0)
            throw new InvalidDataException("The response did not contain a PEM certificate.");

        return results;
    }
}

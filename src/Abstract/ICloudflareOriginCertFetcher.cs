using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;

/// <summary>
/// Cloudflare origin certificate retrieval utility
/// </summary>
public interface ICloudflareOriginCertFetcher
{
    /// <summary>
    /// Downloads the Cloudflare shared AOP certificate PEM and returns its thumbprints.
    /// </summary>
    ValueTask<List<string>> GetSharedAopThumbprints(CancellationToken cancellationToken = default);
}
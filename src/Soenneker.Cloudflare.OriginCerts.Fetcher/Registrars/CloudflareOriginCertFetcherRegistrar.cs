using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher.Registrars;

/// <summary>
/// Cloudflare origin certificate retrieval utility
/// </summary>
public static class CloudflareOriginCertFetcherRegistrar
{
    /// <summary>
    /// Adds <see cref="ICloudflareOriginCertFetcher"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareOriginCertFetcherAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().TryAddSingleton<ICloudflareOriginCertFetcher, CloudflareOriginCertFetcher>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ICloudflareOriginCertFetcher"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareOriginCertFetcherAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().TryAddScoped<ICloudflareOriginCertFetcher, CloudflareOriginCertFetcher>();

        return services;
    }
}
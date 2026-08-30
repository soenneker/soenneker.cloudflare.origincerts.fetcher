[![](https://img.shields.io/nuget/v/soenneker.cloudflare.origincerts.fetcher.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.origincerts.fetcher/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.origincerts.fetcher/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.origincerts.fetcher/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cloudflare.origincerts.fetcher.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.origincerts.fetcher/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.origincerts.fetcher/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.origincerts.fetcher/actions/workflows/codeql.yml)

# Soenneker.Cloudflare.OriginCerts.Fetcher

Downloads Cloudflare's shared Authenticated Origin Pull CA certificate and calculates SHA-256 certificate fingerprints.

## Installation

```bash
dotnet add package Soenneker.Cloudflare.OriginCerts.Fetcher
```

## Registration and usage

```csharp
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Registrars;

services.AddCloudflareOriginCertFetcherAsSingleton();

public sealed class OriginCertificateSource(ICloudflareOriginCertFetcher fetcher)
{
    public ValueTask<string> GetPem(CancellationToken cancellationToken)
    {
        return fetcher.GetSharedAopCertificatePem(cancellationToken);
    }

    public ValueTask<List<string>> GetFingerprints(CancellationToken cancellationToken)
    {
        return fetcher.GetSharedAopThumbprints(cancellationToken);
    }
}
```

`GetSharedAopCertificatePem` returns the downloaded PEM text. `GetSharedAopThumbprints` parses every certificate block and returns uppercase SHA-256 fingerprints of the DER certificates.

HTTP failures, invalid Base64, invalid certificates, and responses without any PEM certificate block throw rather than returning an empty fingerprint list. Callers that publish or persist trust material should validate the result and update atomically so a failed download cannot replace the last known-good data.

This package retrieves the shared Cloudflare Authenticated Origin Pull CA. It does not issue origin certificates and does not configure a web server or Cloudflare zone.

using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using AwesomeAssertions;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Tests.Attributes.Local;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CloudflareOriginCertFetcherTests : HostedUnitTest
{
    private readonly ICloudflareOriginCertFetcher _util;

    public CloudflareOriginCertFetcherTests(Host host) : base(host)
    {
        _util = Resolve<ICloudflareOriginCertFetcher>(true);
    }

    [Test]
    public void Default()
    {

    }

    [Test]
    public void ParsePemThumbprints_should_return_SHA256()
    {
        using RSA rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=Test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        using X509Certificate2 certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(1));
        string expected = Convert.ToHexString(SHA256.HashData(certificate.RawData));

        List<string> result = CloudflareOriginCertFetcher.ParsePemThumbprints(certificate.ExportCertificatePem());

        result.Should()
              .ContainSingle()
              .Which.Should()
              .Be(expected);
    }

    [LocalOnly]
    public async ValueTask GetSharedAopThumbprints_should_get_thumbprints()
    {
        List<string> result = await _util.GetSharedAopThumbprints(System.Threading.CancellationToken.None);
        result.Should().NotBeNullOrEmpty();
    }
}


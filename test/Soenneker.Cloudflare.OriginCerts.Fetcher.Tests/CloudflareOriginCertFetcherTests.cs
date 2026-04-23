using System.Collections.Generic;
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

    [LocalOnly]
    public async ValueTask GetSharedAopThumbprints_should_get_thumbprints()
    {
        List<string> result = await _util.GetSharedAopThumbprints(System.Threading.CancellationToken.None);
        result.Should().NotBeNullOrEmpty();
    }
}


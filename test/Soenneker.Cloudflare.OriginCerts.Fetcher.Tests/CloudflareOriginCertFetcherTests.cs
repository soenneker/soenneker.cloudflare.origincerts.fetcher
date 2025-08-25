using System.Collections.Generic;
using AwesomeAssertions;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Facts.Local;
using Soenneker.Tests.FixturedUnit;
using System.Threading.Tasks;
using Xunit;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher.Tests;

[Collection("Collection")]
public sealed class CloudflareOriginCertFetcherTests : FixturedUnitTest
{
    private readonly ICloudflareOriginCertFetcher _util;

    public CloudflareOriginCertFetcherTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ICloudflareOriginCertFetcher>(true);
    }

    [Fact]
    public void Default()
    {

    }

    [LocalFact]
    public async ValueTask GetSharedAopThumbprints_should_get_thumbprints()
    {
        List<string> result = await _util.GetSharedAopThumbprints(CancellationToken);
        result.Should().NotBeNullOrEmpty();
    }
}

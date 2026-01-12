using System.Text.RegularExpressions;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher;

public partial class CertRegex
{
    [GeneratedRegex("-----BEGIN CERTIFICATE-----(.*?)-----END CERTIFICATE-----", RegexOptions.Singleline)]
    public static partial Regex PemCertRegex();
}
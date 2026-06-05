using System.Text.RegularExpressions;

namespace Soenneker.Cloudflare.OriginCerts.Fetcher;

/// <summary>
/// Represents the cert regex.
/// </summary>
public partial class CertRegex
{
    /// <summary>
    /// Executes the pem cert regex operation.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    [GeneratedRegex("-----BEGIN CERTIFICATE-----(.*?)-----END CERTIFICATE-----", RegexOptions.Singleline)]
    public static partial Regex PemCertRegex();
}
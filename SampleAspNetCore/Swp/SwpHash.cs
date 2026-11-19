using System.Security.Cryptography;
using System.Text;

namespace SampleAspNetCore.Swp;

/// <summary>
/// Builds the Secure Web Payments (SWP) transaction hash.
/// Formula: HMAC-MD5 of
/// api_login_id|transaction_type|version|amount|utc_ticks|order_number
/// using the merchant secure transaction key. Never send that key to the browser or to SWP.
/// </summary>
public static class SwpHash
{
    public const string VersionNumber = "1.0";

    public static string UtcTicks() => DateTime.UtcNow.Ticks.ToString();

    public static string BuildPayload(
        string apiLoginId,
        string transactionType,
        string amount,
        string utcTicks,
        string orderNumber)
    {
        return string.Join('|',
            apiLoginId,
            transactionType,
            VersionNumber,
            amount,
            utcTicks,
            orderNumber);
    }

    public static string CalculateHmacMd5(string payload, string secureTransactionKey)
    {
        var data = Encoding.UTF8.GetBytes(payload);
        var key = Encoding.UTF8.GetBytes(secureTransactionKey);
        using var hmac = new HMACMD5(key);
        return Convert.ToHexString(hmac.ComputeHash(data)).ToLowerInvariant();
    }
}

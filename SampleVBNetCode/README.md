# SWP hosted checkout sample — ASP.NET Web Forms (VB.NET)

This example shows how a merchant site can send a shopper to Secure Web Payments (SWP).

1. Collect checkout fields on your site.
2. On the **server**, build HMAC-MD5 using the merchant secure transaction key.
3. POST only SWP fields to the hosted checkout page. **Never** post the secret key.

It is the classic Web Forms VB.NET sample (`SampleVBNetCode.aspx`). The C# Web Forms equivalent is `SWPCSharpIntegration`; the ASP.NET Core equivalent is `SampleAspNetCore`.

## Hash formula (Secure Transaction mode)

```
HMAC-MD5(
  pg_api_login_id|pg_transaction_type|pg_version_number|pg_total_amount|pg_utc_time|pg_transaction_order_number,
  secure_transaction_key
)
```

- Version is always `1.0`.
- `pg_utc_time` is .NET `DateTime.UtcNow.Ticks` (100-nanosecond intervals since 0001-01-01).
- The digest is lowercase hex.

HTML mode skips the hash and posts billing fields plus the API login ID.

## Run

1. Install [Visual Studio](https://visualstudio.microsoft.com/) with the **ASP.NET and web development** workload and the **.NET Framework 3.5** targeting pack (or upgrade the project to a later Framework version).
2. Open `SampleVBNetCode.sln`.
3. Set `SampleVBNetCode.aspx` as the start page if needed, then press F5 (IIS Express).
4. Replace the placeholder credentials on the form before testing against a live SWP URL.

## Files to copy

| File | Purpose |
|---|---|
| `SampleVBNetCode/SampleVBNetCode.aspx` | Form UI and browser POST to SWP |
| `SampleVBNetCode/SampleVBNetCode.aspx.vb` | Server submit + HMAC-MD5 hash |
| `SampleVBNetCode/css/site.css` | Sample page styles |

Replace `YOUR_API_LOGIN_ID` and `YOUR_SECURE_TRANSACTION_KEY` before testing against a live SWP URL.

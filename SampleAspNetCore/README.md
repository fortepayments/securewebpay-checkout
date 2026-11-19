# SWP hosted checkout sample — ASP.NET Core Razor Pages

This example shows how a merchant site can send a shopper to Secure Web Payments (SWP).

1. Collect checkout fields on your site.
2. On the **server**, build HMAC-MD5 using the merchant secure transaction key.
3. POST only SWP fields to the hosted checkout page. **Never** post the secret key.

It is the ASP.NET Core equivalent of the classic Web Forms `SampleCSharpCode.aspx` sample.

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

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/download).
2. Put your credentials in `appsettings.json` (or type them on the form).
3. From this folder:

```
dotnet run
```

4. Open http://localhost:5080

## Files to copy

| File | Purpose |
|---|---|
| `Swp/SwpHash.cs` | Hash helper |
| `Pages/Index.cshtml.cs` | Server submit + hash |
| `Pages/Index.cshtml` | Form, then auto-POST to SWP |

Replace `YOUR_API_LOGIN_ID` and `YOUR_SECURE_TRANSACTION_KEY` before testing against a live SWP URL.

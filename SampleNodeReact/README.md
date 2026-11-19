# SWP hosted checkout sample — Node.js + React

This example shows a JavaScript stack sending a shopper to Secure Web Payments (SWP).

- **React** collects the checkout fields.
- **Express** creates the HMAC-MD5 hash with the merchant secret key.
- The browser then POSTs only SWP fields to the hosted checkout page. **Never** post the secret key, and never hash in the browser.

## Hash formula (Secure Transaction mode)

```
HMAC-MD5(
  pg_api_login_id|pg_transaction_type|pg_version_number|pg_total_amount|pg_utc_time|pg_transaction_order_number,
  secure_transaction_key
)
```

- Version is always `1.0`.
- `pg_utc_time` matches .NET `DateTime.UtcNow.Ticks`.
- The digest is lowercase hex.

HTML mode skips the hash and posts billing fields plus the API login ID.

## Run

From this folder (`SampleNodeReact`), not from `client` or `server`:

```
npm install
npm run dev
```

Then open http://localhost:5173

That starts the Express API (port 3001) and the React app (port 5173). Vite proxies `/api` to the API.

Optional: copy `server/.env.example` to `server/.env` and add merchant credentials. You can also type them on the form.

In production, keep `SWP_SECURE_TRANSACTION_KEY` only in server environment variables. The React password field is optional and is for local demos.

## Files to copy

| File | Purpose |
|---|---|
| `server/src/hash.js` | Hash helper |
| `server/src/index.js` | `/api/prepare-checkout` |
| `client/src/App.jsx` | Form UI and POST to SWP |

Replace `YOUR_API_LOGIN_ID` and `YOUR_SECURE_TRANSACTION_KEY` before testing against a live SWP URL.

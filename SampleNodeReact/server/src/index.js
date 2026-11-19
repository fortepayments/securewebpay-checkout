const path = require("path");
const express = require("express");
const cors = require("cors");
require("dotenv").config({ path: path.join(__dirname, "..", ".env") });

const { VERSION_NUMBER, utcTicks, buildPayload, hmacMd5 } = require("./hash");

const app = express();
const port = Number(process.env.PORT || 3001);

app.use(cors());
app.use(express.json());

function config() {
  return {
    apiLoginId: process.env.SWP_API_LOGIN_ID || "YOUR_API_LOGIN_ID",
    secureTransactionKey: process.env.SWP_SECURE_TRANSACTION_KEY || "YOUR_SECURE_TRANSACTION_KEY",
    checkoutUrl: process.env.SWP_CHECKOUT_URL || "https://swp.paymentsgateway.net/co/default.aspx",
    returnUrl: process.env.SWP_RETURN_URL || ""
  };
}

app.get("/api/config", (_req, res) => {
  const defaults = config();
  res.json({
    apiLoginId: defaults.apiLoginId,
    checkoutUrl: defaults.checkoutUrl,
    returnUrl: defaults.returnUrl
  });
});

app.post("/api/prepare-checkout", (req, res) => {
  const body = req.body || {};
  const defaults = config();
  const apiLoginId = String(body.apiLoginId || defaults.apiLoginId).trim();
  const secureTransactionKey = String(body.secureTransactionKey || defaults.secureTransactionKey).trim();
  const checkoutUrl = String(body.checkoutUrl || defaults.checkoutUrl).trim();
  const returnUrl = String(body.returnUrl || defaults.returnUrl).trim();
  const company = String(body.company || "").trim();
  const firstName = String(body.firstName || "").trim();
  const lastName = String(body.lastName || "").trim();
  const mode = String(body.mode || "html");
  const transactionType = String(body.transactionType || "").trim();
  const orderNumber = String(body.orderNumber || "").trim();
  const amount = String(body.amount || "").trim();
  const isSecure = mode === "trans";

  if (!apiLoginId || !checkoutUrl) {
    return res.status(400).json({ error: "API Login ID and Checkout URL are required." });
  }

  if (isSecure && (!secureTransactionKey || !orderNumber || !amount || !transactionType)) {
    return res.status(400).json({
      error: "Secure Transaction requires the secret key (form or .env), order number, amount, and transaction type."
    });
  }

  const fields = {
    pg_api_login_id: apiLoginId,
    pg_template_id:200,
    pg_billto_postal_name_company: company,
    pg_billto_postal_name_first: firstName,
    pg_billto_postal_name_last: lastName
  };

  if (returnUrl) {
    fields.pg_return_url = returnUrl;
  }

  if (isSecure) {
    const ticks = utcTicks();
    const payload = buildPayload(apiLoginId, transactionType, amount, ticks, orderNumber);
    fields.pg_ts_hash = hmacMd5(payload, secureTransactionKey);
    fields.pg_transaction_type = transactionType;
    fields.pg_version_number = VERSION_NUMBER;
    fields.pg_utc_time = ticks;
    fields.pg_transaction_order_number = orderNumber;
    fields.pg_total_amount = amount;
  }

  return res.json({ checkoutUrl, fields });
});

app.listen(port, () => {
  console.log(`SWP sample API listening on http://localhost:${port}`);
});

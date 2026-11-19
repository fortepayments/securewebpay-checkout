import { useEffect, useMemo, useState } from "react";

const CC_TYPES = [
  { value: "10", label: "Sale" },
  { value: "11", label: "Auth" },
  { value: "13", label: "Credit" }
];

const EC_TYPES = [
  { value: "20", label: "Sale" },
  { value: "21", label: "Auth" },
  { value: "23", label: "Credit" }
];

function postToSwp(checkoutUrl, fields) {
  const form = document.createElement("form");
  form.method = "POST";
  form.action = checkoutUrl;
  Object.entries(fields).forEach(([name, value]) => {
    const input = document.createElement("input");
    input.type = "hidden";
    input.name = name;
    input.value = value ?? "";
    form.appendChild(input);
  });
  document.body.appendChild(form);
  form.submit();
}

export default function App() {
  const [apiLoginId, setApiLoginId] = useState("YOUR_API_LOGIN_ID");
  const [secureTransactionKey, setSecureTransactionKey] = useState("");
  const [checkoutUrl, setCheckoutUrl] = useState("https://swp.paymentsgateway.net/co/default.aspx");
  const [returnUrl, setReturnUrl] = useState("");
  const [company, setCompany] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [mode, setMode] = useState("html");
  const [paymentMethod, setPaymentMethod] = useState("cc");
  const [transactionType, setTransactionType] = useState("10");
  const [orderNumber, setOrderNumber] = useState("100");
  const [amount, setAmount] = useState("");
  const [error, setError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  const types = useMemo(
    () => (paymentMethod === "ec" ? EC_TYPES : CC_TYPES),
    [paymentMethod]
  );

  useEffect(() => {
    fetch("/api/config")
      .then((response) => response.json())
      .then((data) => {
        if (data.apiLoginId) setApiLoginId(data.apiLoginId);
        if (data.checkoutUrl) setCheckoutUrl(data.checkoutUrl);
        if (data.returnUrl) setReturnUrl(data.returnUrl);
      })
      .catch(() => {
        setError("Could not load API defaults. Start the Express server on port 3001.");
      });
  }, []);

  useEffect(() => {
    if (!types.some((item) => item.value === transactionType)) {
      setTransactionType(types[0].value);
    }
  }, [types, transactionType]);

  async function handleSubmit(event) {
    event.preventDefault();
    setError("");
    setSubmitting(true);
    try {
      const response = await fetch("/api/prepare-checkout", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          apiLoginId,
          secureTransactionKey,
          checkoutUrl,
          returnUrl,
          company,
          firstName,
          lastName,
          mode,
          paymentMethod,
          transactionType,
          orderNumber,
          amount
        })
      });
      const data = await response.json();
      if (!response.ok) {
        throw new Error(data.error || "Could not prepare checkout.");
      }
      postToSwp(data.checkoutUrl, data.fields);
    } catch (err) {
      setError(err.message);
      setSubmitting(false);
    }
  }

  return (
    <main className="wrap">
      <h1>SWP hosted checkout sample</h1>
      <p className="lede">
        React UI plus a Node/Express API. HMAC-MD5 is created on the server, then the browser posts to SWP.
      </p>
      {error ? <div className="error">{error}</div> : null}
      <form onSubmit={handleSubmit}>
        <div className="row">
          <label htmlFor="apiLoginId">API Login ID</label>
          <input id="apiLoginId" value={apiLoginId} onChange={(e) => setApiLoginId(e.target.value)} />
        </div>
        <div className="row">
          <label htmlFor="secureTransactionKey">Secure Transaction Key</label>
          <input
            id="secureTransactionKey"
            type="password"
            autoComplete="off"
            value={secureTransactionKey}
            onChange={(e) => setSecureTransactionKey(e.target.value)}
            placeholder="Leave blank to use server .env"
          />
        </div>
        <div className="row">
          <label htmlFor="checkoutUrl">Checkout URL</label>
          <input id="checkoutUrl" value={checkoutUrl} onChange={(e) => setCheckoutUrl(e.target.value)} />
        </div>
        <div className="row">
          <label htmlFor="returnUrl">Return URL</label>
          <input id="returnUrl" value={returnUrl} onChange={(e) => setReturnUrl(e.target.value)} />
        </div>
        <div className="row">
          <label htmlFor="company">Company Name</label>
          <input id="company" value={company} onChange={(e) => setCompany(e.target.value)} />
        </div>
        <div className="row">
          <label htmlFor="firstName">First Name</label>
          <input id="firstName" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
        </div>
        <div className="row">
          <label htmlFor="lastName">Last Name</label>
          <input id="lastName" value={lastName} onChange={(e) => setLastName(e.target.value)} />
        </div>
        <div className="row">
          <span>Mode</span>
          <div className="radios">
            <label>
              <input type="radio" name="mode" value="html" checked={mode === "html"} onChange={() => setMode("html")} /> HTML
            </label>
            <label>
              <input type="radio" name="mode" value="trans" checked={mode === "trans"} onChange={() => setMode("trans")} /> Secure Transaction
            </label>
          </div>
        </div>
        {mode === "trans" ? (
          <div>
            <div className="row">
              <span>Payment method</span>
              <div className="radios">
                <label>
                  <input type="radio" name="method" value="cc" checked={paymentMethod === "cc"} onChange={() => setPaymentMethod("cc")} /> Credit Card
                </label>
                <label>
                  <input type="radio" name="method" value="ec" checked={paymentMethod === "ec"} onChange={() => setPaymentMethod("ec")} /> eCheck
                </label>
              </div>
            </div>
            <div className="row">
              <label htmlFor="transactionType">Transaction Type</label>
              <select id="transactionType" value={transactionType} onChange={(e) => setTransactionType(e.target.value)}>
                {types.map((item) => (
                  <option key={item.value} value={item.value}>
                    {item.label}
                  </option>
                ))}
              </select>
            </div>
            <div className="row">
              <label htmlFor="orderNumber">Order Number <span className="req">*</span></label>
              <input id="orderNumber" value={orderNumber} onChange={(e) => setOrderNumber(e.target.value)} inputMode="numeric" />
            </div>
            <div className="row">
              <label htmlFor="amount">Total Amount ($) <span className="req">*</span></label>
              <input id="amount" value={amount} onChange={(e) => setAmount(e.target.value)} inputMode="decimal" />
            </div>
          </div>
        ) : null}
        <button type="submit" disabled={submitting}>
          {submitting ? "Redirecting…" : "Submit"}
        </button>
      </form>
      <p className="note">
        The React app never computes the hash. It calls <code>/api/prepare-checkout</code>, then posts the returned fields to SWP.
        Keep the secret key on the server (<code>server/.env</code>) in production.
      </p>
    </main>
  );
}

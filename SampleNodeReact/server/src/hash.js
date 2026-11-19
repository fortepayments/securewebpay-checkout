const crypto = require("crypto");

const VERSION_NUMBER = "1.0";
const DOTNET_EPOCH_TICKS = 621355968000000000n;

/**
 * Matches .NET DateTime.UtcNow.Ticks (100-ns intervals since 0001-01-01).
 */
function utcTicks() {
  return (BigInt(Date.now()) * 10000n + DOTNET_EPOCH_TICKS).toString();
}

function buildPayload(apiLoginId, transactionType, amount, ticks, orderNumber) {
  return [apiLoginId, transactionType, VERSION_NUMBER, amount, ticks, orderNumber].join("|");
}

function hmacMd5(payload, secureTransactionKey) {
  return crypto.createHmac("md5", secureTransactionKey).update(payload, "utf8").digest("hex");
}

module.exports = {
  VERSION_NUMBER,
  utcTicks,
  buildPayload,
  hmacMd5
};

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SampleAspNetCore.Swp;

namespace SampleAspNetCore.Pages;

public class IndexModel : PageModel
{
    private readonly SwpOptions _options;

    public IndexModel(IOptions<SwpOptions> options)
    {
        _options = options.Value;
    }

    [BindProperty] public string PgApiLoginId { get; set; } = string.Empty;
    [BindProperty] public string SecureTransactionKey { get; set; } = string.Empty;
    [BindProperty] public string CheckoutUrl { get; set; } = string.Empty;
    [BindProperty] public string PgReturnUrl { get; set; } = string.Empty;
    [BindProperty] public string PgBilltoPostalNameCompany { get; set; } = string.Empty;
    [BindProperty] public string PgBilltoPostalNameFirst { get; set; } = string.Empty;
    [BindProperty] public string PgBilltoPostalNameLast { get; set; } = string.Empty;
    [BindProperty] public string Mode { get; set; } = "html";
    [BindProperty] public string PaymentMethod { get; set; } = "cc";
    [BindProperty] public string PgTransactionType { get; set; } = "10";
    [BindProperty] public string PgTransactionOrderNumber { get; set; } = "100";
    [BindProperty] public string PgTotalAmount { get; set; } = string.Empty;

    public string PgUtcTime { get; private set; } = string.Empty;
    public string PgTsHash { get; private set; } = string.Empty;
    public bool ReadyToPost { get; private set; }

    public void OnGet()
    {
        LoadDefaults();
    }

    public IActionResult OnPost()
    {
        LoadDefaultsIfEmpty();

        if (string.IsNullOrWhiteSpace(PgApiLoginId) || string.IsNullOrWhiteSpace(CheckoutUrl))
        {
            ModelState.AddModelError(string.Empty, "API Login ID and Checkout URL are required.");
            return Page();
        }

        if (IsSecureTransaction)
        {
            if (string.IsNullOrWhiteSpace(SecureTransactionKey)
                || string.IsNullOrWhiteSpace(PgTransactionOrderNumber)
                || string.IsNullOrWhiteSpace(PgTotalAmount)
                || string.IsNullOrWhiteSpace(PgTransactionType))
            {
                ModelState.AddModelError(string.Empty, "Secure Transaction requires the secret key, order number, amount, and transaction type.");
                return Page();
            }

            PgUtcTime = SwpHash.UtcTicks();
            var payload = SwpHash.BuildPayload(
                PgApiLoginId.Trim(),
                PgTransactionType.Trim(),
                PgTotalAmount.Trim(),
                PgUtcTime,
                PgTransactionOrderNumber.Trim());
            PgTsHash = SwpHash.CalculateHmacMd5(payload, SecureTransactionKey.Trim());
        }

        ReadyToPost = true;
        return Page();
    }

    public bool IsSecureTransaction => string.Equals(Mode, "trans", StringComparison.OrdinalIgnoreCase);

    private void LoadDefaults()
    {
        PgApiLoginId = _options.ApiLoginId;
        SecureTransactionKey = _options.SecureTransactionKey;
        CheckoutUrl = _options.CheckoutUrl;
        PgReturnUrl = _options.ReturnUrl;
        Mode = "html";
        PaymentMethod = "cc";
        PgTransactionType = "10";
        PgTransactionOrderNumber = "100";
    }

    private void LoadDefaultsIfEmpty()
    {
        if (string.IsNullOrWhiteSpace(PgApiLoginId))
            PgApiLoginId = _options.ApiLoginId;
        if (string.IsNullOrWhiteSpace(CheckoutUrl))
            CheckoutUrl = _options.CheckoutUrl;
    }
}

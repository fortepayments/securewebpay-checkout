namespace SampleAspNetCore.Swp;

public class SwpOptions
{
    public string ApiLoginId { get; set; } = "YOUR_API_LOGIN_ID";
    public string SecureTransactionKey { get; set; } = "YOUR_SECURE_TRANSACTION_KEY";
    public string CheckoutUrl { get; set; } = "https://swp.paymentsgateway.net/co/default.aspx";
    public string ReturnUrl { get; set; } = string.Empty;
}

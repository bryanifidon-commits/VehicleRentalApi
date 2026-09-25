using System.Text.Json.Serialization;

namespace Application.Interfaces
{
    public record PaystackInitializeResponse(
        [property: JsonPropertyName("status")] bool Status,
        [property: JsonPropertyName("message")] string Message,
        [property: JsonPropertyName("data")] PaystackInitializeData Data
    );

    public record PaystackInitializeData(
        [property: JsonPropertyName("authorization_url")] string AuthorizationUrl,
        [property: JsonPropertyName("access_code")] string AccessCode,
        [property: JsonPropertyName("reference")] string Reference
    );

    public record PaystackVerifyResponse(
        [property: JsonPropertyName("status")] bool Status,
        [property: JsonPropertyName("message")] string Message,
        [property: JsonPropertyName("data")] PaystackVerifyData Data
    );

    public record PaystackVerifyData(
        [property: JsonPropertyName("status")] string Status, // "success", "failed", etc.
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("amount")] decimal Amount
    );

    public interface IPaystackService
    {
        Task<PaystackInitializeResponse> InitializeTransactionAsync(string email, decimal amount, string reference, string callbackUrl);
        Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference);
    }
}
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class PaystackService : IPaystackService
    {
        private readonly HttpClient _httpClient;

        public PaystackService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["Paystack:BaseUrl"] ?? "https://api.paystack.co/");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", configuration["Paystack:SecretKey"]);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PaystackInitializeResponse> InitializeTransactionAsync(string email, decimal amount, string reference, string callbackUrl)
        {
            // Paystack expects amount in smallest currency unit (Kobo: multiply by 100)
            var payload = new
            {
                email = email,
                amount = ((long)(amount * 100)).ToString(),
                reference = reference,
                callback_url = callbackUrl
            };

            var response = await _httpClient.PostAsJsonAsync("transaction/initialize", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaystackInitializeResponse>();
            return result ?? throw new Exception("Failed to deserialize Paystack initialize response.");
        }

        public async Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference)
        {
            var response = await _httpClient.GetAsync($"transaction/verify/{reference}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaystackVerifyResponse>();
            return result ?? throw new Exception("Failed to deserialize Paystack verify response.");
        }
    }
}
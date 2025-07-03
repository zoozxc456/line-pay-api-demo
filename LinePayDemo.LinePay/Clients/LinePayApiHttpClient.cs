using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LinePayDemo.LinePay.Models;
using LinePayDemo.LinePay.Settings;
using Microsoft.Extensions.Options;

namespace LinePayDemo.LinePay.Clients;

public class LinePayApiHttpClient : ILinePayApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly LinePaySettings _linePaySettings;

    private readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public LinePayApiHttpClient(HttpClient httpClient, IOptions<LinePaySettings> options)
    {
        _httpClient = httpClient;
        _linePaySettings = options.Value;
        _httpClient.BaseAddress = new Uri(_linePaySettings.ApiBaseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelId", _linePaySettings.ChannelId);
    }

    private static string GenerateSignature(string channelSecret, string requestUri, string nonce, string body)
    {
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(channelSecret));
        var data = $"{channelSecret}{requestUri}{body}{nonce}";
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }

    public async Task<LinePayRequestResponse> RequestPayment(LinePayRequest request)
    {
        var requestUri = "/v3/payments/request";
        var nonce = Guid.NewGuid().ToString();
        var jsonBody = JsonSerializer.Serialize(request, _jsonSerializerOptions);
        var signature = GenerateSignature(_linePaySettings.ChannelSecret, requestUri, nonce, jsonBody);

        _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);
        _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUri, content);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LinePayRequestResponse>(responseBody, _jsonSerializerOptions);
    }

    public async Task<LinePayConfirmResponse> ConfirmPayment(long transactionId, LinePayConfirmRequest request)
    {
        var requestUri = $"/v3/payments/{transactionId}/confirm";
        var nonce = Guid.NewGuid().ToString();
        var jsonBody = JsonSerializer.Serialize(request, _jsonSerializerOptions);
        var signature = GenerateSignature(_linePaySettings.ChannelSecret, requestUri, nonce, jsonBody);

        _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);
        _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUri, content);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LinePayConfirmResponse>(responseBody, _jsonSerializerOptions);
    }

    public async Task<LinePayRefundResponse> RefundPayment(long transactionId, LinePayRefundRequest request)
    {
        var requestUri = $"/v3/payments/{transactionId}/refund";
        var nonce = Guid.NewGuid().ToString();
        var jsonBody = JsonSerializer.Serialize(request, _jsonSerializerOptions);
        var signature = GenerateSignature(_linePaySettings.ChannelSecret, requestUri, nonce, jsonBody);

        _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);
        _httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUri, content);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LinePayRefundResponse>(responseBody, _jsonSerializerOptions);
    }
}
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingCardApi.Services.Interfaces;

namespace GreetingCardApi.Services.Implementations;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> PostJsonAsync<T>(string url, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        return response.IsSuccessStatusCode;
    }
}
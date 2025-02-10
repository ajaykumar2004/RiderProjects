using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingCardWebAPI.Models;

namespace GreetingCardWebAPI.Services;
public class GreetingService
{
    private readonly HttpClient _httpClient;

    public GreetingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> PostDataAsync(GreetingCard greetingCard)
    {
        string url = "https://q60qqsjod4.execute-api.us-east-1.amazonaws.com/Demo";

        var jsonContent = JsonSerializer.Serialize(greetingCard);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }
    }
}
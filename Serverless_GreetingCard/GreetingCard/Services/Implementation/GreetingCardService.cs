using GreetingCardApi.Models;
using GreetingCardApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GreetingCardApi.Services.Implementations;

public class GreetingCardService : IGreetingCardService
{
    private readonly IHttpClientService _httpClientService;
    private readonly string _postLambdaUrl;

    public GreetingCardService(IHttpClientService httpClientService, IConfiguration configuration)
    {
        _httpClientService = httpClientService;
        _postLambdaUrl = configuration["AWS:LambdaPostUrl"] ?? throw new ArgumentNullException("AWS:LambdaPostUrl not configured");
    }

    public async Task<bool> SendGreetingCardAsync(GreetingCard card)
    {
        var request = new GreetingCardRequest(card);
        return await _httpClientService.PostJsonAsync(_postLambdaUrl, request);
    }
}
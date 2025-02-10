namespace GreetingCardApi.Services.Interfaces;

public interface IHttpClientService
{
    Task<bool> PostJsonAsync<T>(string url, T data);
}
using GreetingCardApi.Models;

namespace GreetingCardApi.Services.Interfaces;

public interface IDynamoDBService
{
    Task<List<GreetingCard>> GetAllGreetingCardsAsync();
}
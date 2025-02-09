using GreetingCardApi.Models;

namespace GreetingCardApi.Services.Interfaces;

public interface IGreetingCardService
{
    Task<bool> SendGreetingCardAsync(GreetingCard card);
}
namespace GreetingCardApi.Models;

public class GreetingCardRequest
{
    public string Body { get; set; }

    public GreetingCardRequest(GreetingCard card)
    {
        Body = System.Text.Json.JsonSerializer.Serialize(card);
    }
}
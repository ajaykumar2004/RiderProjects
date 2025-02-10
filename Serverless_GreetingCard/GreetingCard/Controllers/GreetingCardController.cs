using GreetingCardApi.Models;
using GreetingCardApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GreetingCardApi.Controllers;

[ApiController]
[Route("api/greetingcards")]
public class GreetingCardController : ControllerBase
{
    private readonly IGreetingCardService _greetingCardService;
    private readonly IDynamoDBService _dynamoDbService;

    public GreetingCardController(IGreetingCardService greetingCardService, IDynamoDBService dynamoDbService)
    {
        _greetingCardService = greetingCardService;
        _dynamoDbService = dynamoDbService;
    }

    [HttpPost]
    public async Task<IActionResult> SendGreetingCard([FromBody] GreetingCard card)
    {
        var result = await _greetingCardService.SendGreetingCardAsync(card);
        if (result)
            return Ok(new { message = "Greeting card sent successfully!" });

        return BadRequest(new { error = "Failed to send greeting card." });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGreetingCards()
    {
        var greetings = await _dynamoDbService.GetAllGreetingCardsAsync();
        return Ok(greetings);
    }
}
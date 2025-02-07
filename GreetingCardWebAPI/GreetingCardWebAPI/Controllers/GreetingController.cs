using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using GreetingCardWebAPI.Models;
using GreetingCardWebAPI.Services;
using Microsoft.CodeAnalysis.Elfie.Serialization;

[ApiController]
[Route("api/[controller]")]
public class GreetingController : ControllerBase
{
    private readonly GreetingService _apiService;
    private readonly GreetingTableService _greetingTableService;

    public GreetingController(GreetingService apiService,GreetingTableService greetingTableService)
    {
        _apiService = apiService;
        _greetingTableService = greetingTableService;
    }

    [HttpPost("sendmessage")]
    public async Task<IActionResult> SendPostRequest([FromBody] GreetingCard greetingCard)
    {
        try
        {
            string response = await _apiService.PostDataAsync(greetingCard);
            return Ok(new { success = true, response });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }

    [HttpGet("getMessages/{to}")]
    public async Task<IActionResult> GetMyMessages(string to)
    {
        var mymessages = await _greetingTableService.GetMessages(to);
        if(mymessages == null)
        {
            return NotFound(new { success = false, error = "No messages found for the user" });
        }
        return Ok(mymessages); 
    }
}
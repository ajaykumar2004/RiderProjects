using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GreetingCardAPI.Models;
namespace GreetingCardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        // Inject IHttpClientFactory into the controller
        public GreetingController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // POST /sendMessage
        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] GreetingCard greetingCard)
        {
            // Check for missing or invalid data
            if (greetingCard == null || string.IsNullOrEmpty(greetingCard.Message) || string.IsNullOrEmpty(greetingCard.From) || string.IsNullOrEmpty(greetingCard.To))
                return BadRequest("Invalid greeting card data.");

            // Set the CreatedAt timestamp to now if it's not provided
            if (greetingCard.CreatedAt == default)
            {
                greetingCard.CreatedAt = DateTime.UtcNow;
            }

            // Prepare the HTTP request to Lambda 1
            var client = _clientFactory.CreateClient();

            var json = JsonConvert.SerializeObject(greetingCard);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("LAMBDA_1_URL", content); // Replace LAMBDA_1_URL with actual Lambda 1 URL.

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Greeting card sent successfully." });
            }

            return StatusCode(500, "Error sending greeting card.");
        }
    }
}
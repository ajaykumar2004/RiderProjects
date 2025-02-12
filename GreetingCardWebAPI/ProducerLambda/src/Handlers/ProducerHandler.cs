using System;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using ProducerLambda.Interfaces;
using ProducerLambda.Models;

namespace ProducerLambda.Handlers
{
    public class ProducerHandler
    {
        private readonly ISqsService _sqsService;

        public ProducerHandler(ISqsService sqsService)
        {
            _sqsService = sqsService ?? throw new ArgumentNullException(nameof(sqsService));
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                context.Logger.LogInformation($"Received request: {request.Body}");

                if (string.IsNullOrWhiteSpace(request.Body))
                {
                    return new APIGatewayProxyResponse { StatusCode = 400, Body = "Request body is missing" };
                }

                // Check if the body is Base64 encoded
                string requestBody = request.IsBase64Encoded 
                    ? Encoding.UTF8.GetString(Convert.FromBase64String(request.Body)) 
                    : request.Body;

                JObject jsonBody;
                try
                {
                    jsonBody = JObject.Parse(requestBody);
                }
                catch (Exception jsonEx)
                {
                    context.Logger.LogError($"JSON parsing error: {jsonEx.Message}");
                    return new APIGatewayProxyResponse { StatusCode = 400, Body = "Invalid JSON format" };
                }

                var card = new GreetingCard
                {
                    From = jsonBody["From"]?.ToString(),
                    To = jsonBody["To"]?.ToString(),
                    Message = jsonBody["Message"]?.ToString(),
                    CreatedAt = DateTime.UtcNow // Always set CreatedAt to avoid null values
                };

                if (string.IsNullOrEmpty(card.From) || string.IsNullOrEmpty(card.To) || string.IsNullOrEmpty(card.Message))
                {
                    return new APIGatewayProxyResponse { StatusCode = 400, Body = "Invalid request payload" };
                }

                bool success = await _sqsService.SendMessageAsync(card);

                return new APIGatewayProxyResponse
                {
                    StatusCode = success ? 200 : 500,
                    Body = success ? "{\"message\": \"Message sent successfully\"}" : "{\"message\": \"Failed to send message\"}"
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error: {ex}");
                return new APIGatewayProxyResponse { StatusCode = 500, Body = $"Internal server error: {ex.Message}" };
            }
        }
    }
}

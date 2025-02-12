using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using ConsumerLambda.Interfaces;
using ConsumerLambda.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ConsumerLambda.Handlers
{
    public class ConsumerHandler
    {
        private readonly IDynamoDbService _dynamoDbService;

        public ConsumerHandler(IDynamoDbService dynamoDbService)
        {
            _dynamoDbService = dynamoDbService;
        }

        public async Task ProcessMessagesAsync(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {
                try
                {
                    context.Logger.LogInformation($"Processing SQS message: {record.Body}");
                    
                    var card = JsonConvert.DeserializeObject<GreetingCard>(record.Body);
                    card.ReceivedAt = DateTime.UtcNow;

                    await _dynamoDbService.SaveGreetingCardAsync(card);
                    
                    context.Logger.LogInformation("Successfully saved to DynamoDB.");
                }
                catch (Exception ex)
                {
                    context.Logger.LogError($"Error processing message: {ex.Message}");
                }
            }
        }
    }
}
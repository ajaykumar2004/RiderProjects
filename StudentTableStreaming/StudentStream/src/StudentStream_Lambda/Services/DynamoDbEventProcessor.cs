using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EnrichmentLambda_StudentStream.Models;

namespace EnrichmentLambda_StudentStream
{
    public class DynamoDbEventProcessor : IDynamoDbEventProcessor
    {
        private readonly ISqsService _sqsService;

        public DynamoDbEventProcessor(ISqsService sqsService)
        {
            _sqsService = sqsService;
        }

        public async Task ProcessEventAsync(string rawJson, ILambdaContext context)
        {
            string wrappedJson = $"{{ \"Records\": {rawJson} }}";
            var customDynamoDbEvent = JsonSerializer.Deserialize<CustomDynamoDBEvent>(wrappedJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (customDynamoDbEvent?.Records == null || customDynamoDbEvent.Records.Count == 0)
            {
                context.Logger.LogWarning("No records found in the event.");
                return;
            }

            foreach (var record in customDynamoDbEvent.Records)
            {
                if (record.Dynamodb?.NewImage == null)
                {
                    context.Logger.LogWarning("Skipping record with no NewImage.");
                    continue;
                }

                var id = record.Dynamodb.NewImage.TryGetValue("id", out var idValue) ? idValue.N : "Unknown";
                var age = record.Dynamodb.NewImage.TryGetValue("age", out var ageValue) ? ageValue.N : "Unknown";

                context.Logger.LogInformation($"New User Added - ID: {id}, Age: {age}");

                var messageBody = new
                {
                    Id = id,
                    Age = age,
                    EventID = record.EventID,
                    EventName = record.EventName,
                    Timestamp = DateTime.UtcNow.ToString("o")
                };

                await _sqsService.SendMessageAsync(messageBody, context);
            }
        }
    }
}

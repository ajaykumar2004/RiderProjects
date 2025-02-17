using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

// Make sure AWS Lambda can find this assembly
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ListensToSQS
{
    public class Function
    {
        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            context.Logger.LogInformation($"Received {sqsEvent.Records.Count} messages.");

            foreach (var record in sqsEvent.Records)
            {
                var body = record.Body;
                context.Logger.LogInformation($"Raw SQS Message: {body}");

                try
                {
                    var dynamoDbEvent = JsonSerializer.Deserialize<DynamoDbStreamEvent>(body);
                    
                    if (dynamoDbEvent?.Dynamodb?.NewImage != null)
                    {
                        var id = dynamoDbEvent.Dynamodb.NewImage["id"].N;
                        var age = dynamoDbEvent.Dynamodb.NewImage["age"].N;
                        
                        context.Logger.LogInformation($"Extracted Data - ID: {id}, Age: {age}");
                    }
                    else
                    {
                        context.Logger.LogWarning("No NewImage found in DynamoDB event.");
                    }
                }
                catch (Exception ex)
                {
                    context.Logger.LogError($"Error parsing SQS message: {ex.Message}");
                }
            }

            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Models the structure of the incoming DynamoDB Stream event.
    /// </summary>
    public class DynamoDbStreamEvent
    {
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string EventSource { get; set; }
        public string AWSRegion { get; set; }
        public DynamoDbStreamDetail Dynamodb { get; set; }
    }

    public class DynamoDbStreamDetail
    {
        public Dictionary<string, DynamoDbAttribute> Keys { get; set; }
        public Dictionary<string, DynamoDbAttribute> NewImage { get; set; }
    }

    public class DynamoDbAttribute
    {
        public string N { get; set; }  // For numeric attributes like "id" and "age"
    }
}

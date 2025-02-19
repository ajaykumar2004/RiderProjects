using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnrichmentLambda_StudentStream
{
    public class SqsService : ISqsService
    {
        private readonly AmazonSQSClient _sqsClient;
        private const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/686255973717/DemoQueue";

        public SqsService()
        {
            _sqsClient = new AmazonSQSClient();
        }

        public async Task SendMessageAsync(object messageBody, ILambdaContext context)
        {
            try
            {
                string messageJson = JsonSerializer.Serialize(messageBody);
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = QueueUrl,
                    MessageBody = messageJson
                };

                var response = await _sqsClient.SendMessageAsync(sendMessageRequest);
                context.Logger.LogInformation($"Message sent to SQS. MessageId: {response.MessageId}");
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Failed to send message to SQS: {ex.Message}");
            }
        }
    }
}
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using ProducerLambda.Interfaces;
using ProducerLambda.Models;

namespace ProducerLambda.Services
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public SqsService(IAmazonSQS sqsClient, IConfiguration configuration)
        {
            _sqsClient = sqsClient;
            _queueUrl = configuration["SqsQueueUrl"] ?? throw new ArgumentNullException("SqsQueueUrl is missing in config");
        }

        public async Task<bool> SendMessageAsync(GreetingCard card)
        {
            var messageBody = JsonSerializer.Serialize(card);
            
            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = messageBody
            };

            var response = await _sqsClient.SendMessageAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
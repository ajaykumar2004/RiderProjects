using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ConsumerLambda.Interfaces;
using ConsumerLambda.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerLambda.Services
{
    public class DynamoDbService : IDynamoDbService
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private const string TableName = "GreetingCards";

        public DynamoDbService()
        {
            _dynamoDbClient = new AmazonDynamoDBClient();
        }

        public async Task SaveGreetingCardAsync(GreetingCard card)
        {
            var request = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "To", new AttributeValue { S = card.To } },
                    { "From", new AttributeValue { S = card.From } },
                    { "Message", new AttributeValue { S = card.Message } },
                    { "CreatedAt", new AttributeValue { S = card.CreatedAt.ToString("o") } },
                    { "ReceivedAt", new AttributeValue { S = card.ReceivedAt.ToString("o") } }
                }
            };

            await _dynamoDbClient.PutItemAsync(request);
        }
    }
}
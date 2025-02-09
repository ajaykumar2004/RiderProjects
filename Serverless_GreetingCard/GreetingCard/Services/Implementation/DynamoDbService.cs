using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using GreetingCardApi.Models;
using GreetingCardApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GreetingCardApi.Services.Implementations;

public class DynamoDBService : IDynamoDBService
{
    private readonly AmazonDynamoDBClient _dynamoDbClient;
    private readonly string _tableName;

    public DynamoDBService(IConfiguration configuration)
    {
        _tableName = configuration["AWS:DynamoDBTable"] ?? throw new ArgumentNullException("AWS:DynamoDBTable not configured");

        _dynamoDbClient = new AmazonDynamoDBClient();
    }

    public async Task<List<GreetingCard>> GetAllGreetingCardsAsync()
    {
        var request = new ScanRequest { TableName = _tableName };
        var response = await _dynamoDbClient.ScanAsync(request);

        return response.Items.Select(item => new GreetingCard
        {
            From = item["From"].S,
            To = item["To"].S,
            Message = item["Message"].S,
            CreatedAt = DateTime.Parse(item["CreatedAt"].S)
        }).ToList();
    }
}
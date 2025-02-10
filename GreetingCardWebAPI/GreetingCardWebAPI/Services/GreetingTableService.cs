using Amazon.DynamoDBv2.DataModel;
using GreetingCardWebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GreetingCardWebAPI.Services;

public class GreetingTableService
{
    private readonly IDynamoDBContext _dynamoDbContext;
    public GreetingTableService(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }
    public async Task<List<DynamoDBItem>> GetMessages(string id)
    {
        var queryResult = await _dynamoDbContext.QueryAsync<DynamoDBItem>(id).GetRemainingAsync();
        queryResult.Sort((a, b) => DateTime.Compare(Convert.ToDateTime(b.CreatedAt), Convert.ToDateTime(a.CreatedAt)));
        return queryResult;
    }

}
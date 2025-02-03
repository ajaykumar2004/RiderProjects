using Amazon.DynamoDBv2.DataModel;
namespace DynamoDB_MovieAPI.Models;
[DynamoDBTable("MoviesBase")]
public class Movie
{
    [DynamoDBHashKey("Id")]
    public int Id { get; set; }
    [DynamoDBProperty("Director")]
    public string Director { get; set; }
    [DynamoDBProperty("Name")]
    public string Name { get; set; }
    [DynamoDBProperty("ReleasedDate")]
    public string ReleasedDate { get; set; }
}
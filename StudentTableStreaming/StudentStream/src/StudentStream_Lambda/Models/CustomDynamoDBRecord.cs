namespace EnrichmentLambda_StudentStream.Models;
public class CustomDynamoDBRecord
{
    public string EventID { get; set; }
    public string EventName { get; set; }
    public CustomDynamoDBStream Dynamodb { get; set; }
}
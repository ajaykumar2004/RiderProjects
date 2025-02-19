namespace EnrichmentLambda_StudentStream.Models;
public class CustomDynamoDBEvent
{
    public List<CustomDynamoDBRecord> Records { get; set; }
}
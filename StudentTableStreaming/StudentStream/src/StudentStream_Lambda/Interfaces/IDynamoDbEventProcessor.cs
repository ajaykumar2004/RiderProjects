using Amazon.Lambda.Core;
namespace EnrichmentLambda_StudentStream
{
    public interface IDynamoDbEventProcessor
    {
        Task ProcessEventAsync(string rawJson, ILambdaContext context);
    }
}
using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace EnrichmentLambda_StudentStream
{
    public interface ISqsService
    {
        Task SendMessageAsync(object messageBody, ILambdaContext context);
    }
}
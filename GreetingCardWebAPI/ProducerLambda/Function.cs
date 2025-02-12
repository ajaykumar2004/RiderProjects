using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ProducerLambda.Handlers;
using ProducerLambda.Interfaces;
using ProducerLambda.Services;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace ProducerLambda
{
    public class Function
    {
        private readonly ProducerHandler _handler;

        public Function()
        {
            var serviceProvider = ConfigureServices();
            _handler = serviceProvider.GetRequiredService<ProducerHandler>();
        }

        private static ServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            return new ServiceCollection()
                .AddSingleton<IAmazonSQS, AmazonSQSClient>()  // ✅ Register SQS Client
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<ISqsService, SqsService>()
                .AddSingleton<ProducerHandler>()  // ✅ Register ProducerHandler
                .BuildServiceProvider();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return await _handler.HandleAsync(request, context);
        }
    }
}
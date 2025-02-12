using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Threading.Tasks;
using ConsumerLambda.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ConsumerLambda.Interfaces;
using ConsumerLambda.Services;
// Register the serializer (uses System.Text.Json)
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ConsumerLambda
{
    public class Function
    {
        private readonly ConsumerHandler _handler;

        public Function()
        {
            var serviceProvider = ConfigureServices();
            _handler = serviceProvider.GetRequiredService<ConsumerHandler>();
        }

        private static ServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            return new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<IDynamoDbService, DynamoDbService>()
                .AddSingleton<ConsumerHandler>()
                .BuildServiceProvider();
        }

        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            await _handler.ProcessMessagesAsync(sqsEvent, context);
        }
    }
}

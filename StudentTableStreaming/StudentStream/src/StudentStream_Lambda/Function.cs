using Amazon.Lambda.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EnrichmentLambda_StudentStream
{
    public class Function
    {
        private readonly IDynamoDbEventProcessor _eventProcessor;

        public Function() : this(DependencyInjector.Resolve<IDynamoDbEventProcessor>())
        {
        }

        public Function(IDynamoDbEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
        }

        public async Task FunctionHandler(Stream inputStream, ILambdaContext context)
        {
            using var reader = new StreamReader(inputStream);
            string rawJson = await reader.ReadToEndAsync();
            context.Logger.LogInformation($"Raw Event Received: {rawJson}");

            try
            {
                await _eventProcessor.ProcessEventAsync(rawJson, context);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Processing Failed: {ex.Message}");
                throw;
            }
        }
    }
}
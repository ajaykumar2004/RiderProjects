using System;

namespace EnrichmentLambda_StudentStream
{
    public static class DependencyInjector
    {
        public static T Resolve<T>()
        {
            if (typeof(T) == typeof(IDynamoDbEventProcessor))
                return (T)(object)new DynamoDbEventProcessor(new SqsService());
            if (typeof(T) == typeof(ISqsService))
                return (T)(object)new SqsService();
            
            throw new InvalidOperationException($"No implementation found for {typeof(T).Name}");
        }
    }
}
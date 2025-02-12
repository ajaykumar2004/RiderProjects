using System.Threading.Tasks;
using ConsumerLambda.Models;

namespace ConsumerLambda.Interfaces
{
    public interface IDynamoDbService
    {
        Task SaveGreetingCardAsync(GreetingCard card);
    }
}

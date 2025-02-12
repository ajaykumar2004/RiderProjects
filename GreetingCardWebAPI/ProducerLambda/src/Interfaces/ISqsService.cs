using System.Threading.Tasks;
using ProducerLambda.Models;

namespace ProducerLambda.Interfaces
{
    public interface ISqsService
    {
        Task<bool> SendMessageAsync(GreetingCard card);
    }
}
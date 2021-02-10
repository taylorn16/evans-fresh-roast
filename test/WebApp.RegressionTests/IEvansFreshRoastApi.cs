using System.Threading.Tasks;
using Refit;

namespace WebApp.RegressionTests
{
    public interface IEvansFreshRoastApi
    {
        [Post("/api/v1/contacts")]
        Task<Dto.Contact> CreateContact([Body] Dto.ContactPostRequest request);

        [Post("/api/v1/events")]
        Task<Dto.CoffeeRoastingEvent> CreateEvent([Body] Dto.CoffeeRoastingEventPostRequest request);
    }
}

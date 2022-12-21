using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public interface ISubscriptionQuery
    {
        public Task<UserCourseDTO> GetSubscriptionById(string Id);
        public Task<UserCourseDTO> FindSubscription(SubscriptionRequestDTO request);
        public Task<SubscriptionResponseDTO> GetAllUserSubscriptions(string UserId);
    }
}

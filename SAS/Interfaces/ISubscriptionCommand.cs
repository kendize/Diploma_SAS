using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public interface ISubscriptionCommand
    {
        public Task<UserCourseDTO> Subscribe(UserCourseDTO userCourse);
        public Task<UserCourseDTO> UnSubscribe(UserCourseDTO subscription);
    }
}

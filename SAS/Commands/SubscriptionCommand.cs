using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public class SubscriptionCommand : ISubscriptionCommand
    {
        private readonly ApplicationContext db;

        public SubscriptionCommand(ApplicationContext context)
        {
            db = context;

        }
        public async Task<UserCourseDTO> Subscribe(UserCourseDTO userCourse)
        {

            await db.UserCourses.AddAsync(userCourse);
            await db.SaveChangesAsync();
            return userCourse;
        }

        public async Task<UserCourseDTO> UnSubscribe(UserCourseDTO subscription)
        {
            db.UserCourses.Remove(subscription);
            await db.SaveChangesAsync();
            return subscription;
        }

    }
}

using SAS.DTO;
using System;
using System.Threading.Tasks;


namespace SAS.Interfaces
{
    public interface ISubscriptionService
    {
        public Task<bool> CheckNotification(UserDTO user,
                                            CourseDTO course,
                                            UserCourseDTO userCourse,
                                            DateTime notificationTime,
                                            int daysLeft);
        public Task<UserCourseDTO> Subscribe(SubscriptionRequestDTO request);

        public Task<UserCourseDTO> UnSubscribe(SubscriptionRequestDTO request);
    }
}

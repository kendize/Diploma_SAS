using Hangfire;
using SAS.DTO;
using SAS.Queries;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace SAS.Interfaces
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationContext db;
        private readonly IUserQuery userQuery;
        private readonly IUserCommand userCommand;
        private readonly ICourseQuery courseQuery;
        private readonly ISubscriptionCommand subscriptionCommand;
        private readonly ISubscriptionQuery subscriptionQuery;
        private readonly INotificationService notificationService;

        public SubscriptionService(ApplicationContext context,
                            IUserQuery userQuery,
                            IUserCommand userCommand,
                            ICourseQuery courseQuery,
                            ISubscriptionCommand subscriptionCommand,
                            ISubscriptionQuery subscriptionQuery,
                            INotificationService notificationService)
        {
            db = context;
            this.userQuery = userQuery;
            this.userCommand = userCommand;
            this.courseQuery = courseQuery;
            this.subscriptionCommand = subscriptionCommand;
            this.subscriptionQuery = subscriptionQuery;
            this.notificationService = notificationService;
        }
        public async Task<UserCourseDTO> Subscribe(SubscriptionRequestDTO request)
        {
            DateTime now = DateTime.Now;

            if (now > DateTime.Parse(request.StudyDate))
            {
                return null;
            }
            const int day = 1;
            const int week = 7;
            const int month = 30;
            
            
            DateTime dayBefore = DateTime.Parse(request.StudyDate).AddDays(-day);
            DateTime weekBefore = DateTime.Parse(request.StudyDate).AddDays(-week);
            DateTime monthBefore = DateTime.Parse(request.StudyDate).AddDays(-month);


            UserDTO user = await userQuery.GetUserById(request.UserId);
            CourseDTO course = await courseQuery.GetCourseById(request.CourseId);
            if (user == null || course == null)                                
            {
                return null;
            }

            UserCourseDTO subscription = await subscriptionQuery.FindSubscription(request);
            if (subscription == null)
            {
                var userCourse = new UserCourseDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    Course = course,
                    CourseId = course.Id,
                    User = user,
                    UserId = user.Id,                                                     //
                    StudyDate = DateTime.Parse(request.StudyDate)
                };

                await CheckNotification(user, course, userCourse, dayBefore, day);
                await CheckNotification(user, course, userCourse, weekBefore, week);
                await CheckNotification(user, course, userCourse, monthBefore, month);
                var result = await subscriptionCommand.Subscribe(userCourse);
                return result;
            }

            return null;
            
        }

        public async Task<bool> CheckNotification(UserDTO user,
                                                  CourseDTO course,
                                                  UserCourseDTO userCourse,
                                                  DateTime notificationTime,
                                                  int daysLeft)
        {
            DateTime now = DateTime.Now;
            if (now < notificationTime)
            {
                var CourseNotificationModel = new CourseNotificationDTO
                {
                    Email = user.Email,
                    Days = daysLeft,
                    CourseName = course.CourseName
                };

                var Notification = new HangfireJobDTO
                {
                    userCourse = userCourse,
                    Id = notificationService.ScheduleNotification(CourseNotificationModel, notificationTime),
                    userCourseId = userCourse.Id
                };

                await db.HangfireJob.AddAsync(Notification);
                return true;
            }

            return false;

        }

        public async Task<UserCourseDTO> UnSubscribe(SubscriptionRequestDTO request)
        {

            UserCourseDTO subscription = await subscriptionQuery.FindSubscription(request);     // Перевіряємо наявність підписки
            if (subscription != null)
            {
                var HangfireJobs = db.HangfireJob.Where(model => model.userCourseId.Contains(subscription.Id));
                foreach (var Job in HangfireJobs)
                {
                    BackgroundJob.Delete(Job.Id);
                }

                var result = await subscriptionCommand.UnSubscribe(subscription);
                return result;
            }

            return null;
            
        }


    }
}

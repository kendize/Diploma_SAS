using Hangfire;
using SAS.DTO;
using SAS.Interfaces;
using System;

namespace SAS.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService emailService;
        public NotificationService(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public string ScheduleNotification(CourseNotificationDTO model, DateTime date)
        {
            var jobId = BackgroundJob.Schedule(
                () => emailService.SendCourseNotification(model),
                date
                );
            return jobId;
        }
    }
}

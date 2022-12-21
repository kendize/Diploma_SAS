using SAS.DTO;
using System;

namespace SAS.Interfaces
{
    public interface INotificationService
    {
        public string ScheduleNotification(CourseNotificationDTO model, DateTime date);

    }
}

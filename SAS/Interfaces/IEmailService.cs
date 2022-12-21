using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Interfaces
{
    public interface IEmailService
    {
        public Task Send(string email, string subject, string template);
        public Task SendEmailConfirmation(EmailConfirmationDTO model);
        public Task SendCourseNotification(CourseNotificationDTO model);
    }
}

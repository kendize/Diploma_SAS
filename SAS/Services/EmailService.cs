using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using SAS.DTO;
using SAS.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace SAS.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettingsDTO mailSettings;
        private readonly ITemplateHelper templateHelper;
        public EmailService(IConfiguration configuration,
                            IOptions<MailSettingsDTO> mailSettings,
                            ITemplateHelper templateHelper)
        {
            Configuration = configuration;
            this.mailSettings = mailSettings.Value;
            this.templateHelper = templateHelper;
        }

        public IConfiguration Configuration { get; }
        public async Task Send(string email, string subject, string template)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(mailSettings.From, mailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = template };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailSettings.Host,
                                          mailSettings.Port,
                                          MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(mailSettings.Email, mailSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailConfirmation(EmailConfirmationDTO model)
        {
            var path = Path.Combine("EmailTemplates",
                                    "EmailConfirmTemplate");

            var result = await templateHelper.GetTemplateHtmlAsStringAsync<EmailConfirmationDTO>(path, model);
            await Send(model.Email, "Confirm your Email", result);
        }

        public async Task SendCourseNotification(CourseNotificationDTO model)
        {
            var path = Path.Combine("EmailTemplates",
                                    "CourseNotificationTemplate");
            var result = await templateHelper.GetTemplateHtmlAsStringAsync<CourseNotificationDTO>(path, model);
            await Send(model.Email, "Study Notification", result);
        }
    }
}

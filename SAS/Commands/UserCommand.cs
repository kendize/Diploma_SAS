using FluentValidation.Results;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NLog;
using SAS.DTO;
using SAS.Interfaces;
using SAS.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SAS.Queries
{
    public class UserCommand : IUserCommand
    {
        private readonly IUserQuery userQuery;
        private readonly UserManager<UserDTO> userManager;
        private readonly ApplicationContext db;
        private readonly IEmailService emailService;
        private readonly ILogger<CourseCommand> log;

        public UserCommand(IUserQuery userQuery,
                            UserManager<UserDTO> manager,
                            ApplicationContext context,
                            IEmailService emailService,
                            ILogger<CourseCommand> log)
        {
            this.userQuery = userQuery;
            userManager = manager;
            db = context;
            this.emailService = emailService;
            this.log = log;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            //code = HttpUtility.UrlDecode(code);
            if (userId == null || code == null)
            {
                return false;
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return true;
            else
                return false;
        }


        public async Task<UserDTO> CreateNewUserAsync(RegistrationDTO RegistrationUser)
        {
            var validator = new RegistrationDTOValidator();
            ValidationResult validationResult = validator.Validate(RegistrationUser);

            if (!validationResult.IsValid)
            {
                return null;
            }

            UserDTO existing_user = await userQuery.GetUserByEmail(RegistrationUser.Email);
            if (existing_user != null)
            {
                return null;
            }

            


            UserDTO ResultUser = new UserDTO
            {
                Id = Guid.NewGuid().ToString(),
                UserName = RegistrationUser.Email,
                FirstName = RegistrationUser.FirstName,
                LastName = RegistrationUser.LastName,
                Age = RegistrationUser.Age,
                Email = RegistrationUser.Email,
                RegisteredDate = @DateTime.UtcNow.ToShortDateString()
            };

            var result = await userManager.CreateAsync(ResultUser, RegistrationUser.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            result = await userManager.AddToRoleAsync(ResultUser, "user");

            if (!result.Succeeded)
            {
                return null;
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(ResultUser);

            var EmailConfirmationModel = new EmailConfirmationDTO
            {
                Email = ResultUser.Email,
                Token = HttpUtility.UrlEncode(code),
                Id = ResultUser.Id
            };

            await emailService.SendEmailConfirmation(EmailConfirmationModel);
            return ResultUser;
        }

        public async Task<UserDTO> CreateNewFacebookUserAsync(FacebookDataDTO user)
        {
            UserDTO existing_user = await userQuery.GetUserById(user.id);
            if (existing_user != null)
            {
                return null;
            }

            var ResultUser = new UserDTO()
            {
                Id = user.id,
                UserName = user.id,
                FirstName = user.first_name,
                LastName = user.last_name,
                Email = user.email,
                RegisteredDate = @DateTime.UtcNow.ToShortDateString()
            };

            await userManager.CreateAsync(ResultUser);
            await userManager.AddToRoleAsync(ResultUser, "user");
            return ResultUser;
        }

        public async Task<UserDTO> DeleteSingleUserAsync(string Id)
        {
            UserDTO user = await userQuery.GetUserById(Id);

            if (user != null)
            {
                var userCourses = db.UserCourses.Where(model => model.UserId.Contains(Id));
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.UserCourses.RemoveRange(userCourses);
                        db.Users.Remove(user);
                        foreach (var subscription in userCourses)
                        {
                            var HangfireJobs = db.HangfireJob.Where(model => model.userCourseId.Contains(subscription.Id));
                            foreach (var Job in HangfireJobs)
                            {
                                BackgroundJob.Delete(Job.Id);
                            }
                        }

                        await db.SaveChangesAsync();
                        transaction.Commit();
                        return user;
                    }

                    catch (Exception ex)
                    {
                        log.LogError(ex, "Error occured while deleting user {0}", user.Id);
                        transaction.Rollback();
                        return null;
                    }
                }
            }

            return null;
        }

        public async Task<UserDTO> EditUserAsync(UserDTO user)
        {
            UserDTO existing_user = await userQuery.GetUserById(user.Id);
            if (existing_user != null)
            {
                existing_user.FirstName = user.FirstName;
                existing_user.LastName = user.LastName;
                existing_user.Age = user.Age;
                existing_user.Email = user.Email;

                await userManager.UpdateAsync(existing_user);
                return existing_user;
            }

            return null;
        }
    }
}

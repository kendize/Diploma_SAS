using FluentValidation.Results;
using Hangfire;
using Microsoft.Extensions.Logging;
using NLog;
using SAS.DTO;
using SAS.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public class CourseCommand : ICourseCommand
    {
        private readonly ApplicationContext db;
        private readonly ICourseQuery courseQuery;
        private readonly ILogger<CourseCommand> log;

        public CourseCommand(ApplicationContext context, 
                             ICourseQuery courseQuery,
                             ILogger<CourseCommand> log)
        {
            db = context;
            this.courseQuery = courseQuery;
            this.log = log;
        }

        public async Task<CourseDTO> CreateNewCourseAsync(CourseDTO course)
        {
            var validator = new CourseDTOValidator();
            ValidationResult validationResult = validator.Validate(course);

            if (!validationResult.IsValid)
            {
                return null;
            }

            var existingCourse = await courseQuery.GetCourseByName(course.CourseName);
            if (existingCourse != null)
            {
                log.LogError("Course '{0}' already exist, new course wasn't created.", existingCourse.CourseName);

                return null;
            }

            

            var result = new CourseDTO
            {
                Id = Guid.NewGuid().ToString(),
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                CourseImgUrl = course.CourseImgUrl
            };

            db.Courses.Add(result);
            await db.SaveChangesAsync();
            return result;
        }
        public async Task<CourseDTO> EditCourseAsync(CourseDTO course)
        {
            CourseDTO result = await courseQuery.GetCourseById(course.Id);
            if (result != null)
            {
                result.CourseName = course.CourseName;
                result.CourseDescription = course.CourseDescription;
                result.CourseImgUrl = course.CourseImgUrl;
                db.Courses.Update(result);
                await db.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<CourseDTO> DeleteSingleCourseAsync(string id)
        {
            CourseDTO result = await courseQuery.GetCourseById(id);
            if (result != null)
            {
                var userCourses = db.UserCourses.Where(model => model.CourseId.Contains(id));
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.UserCourses.RemoveRange(userCourses);
                        db.Courses.Remove(result);
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
                        return result;
                    }

                    catch (Exception ex)
                    {
                        log.LogError(ex, "Error occured while deleting course {0}", result.CourseName);
                        transaction.Rollback();
                        return null;
                    }
                }
            }

            return null;

        }
    }
}

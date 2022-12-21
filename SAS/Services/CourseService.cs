using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SAS.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseQuery courseQuery;
        private readonly ICourseCommand courseCommand;

        public CourseService(ICourseQuery courseQuery,
                             ICourseCommand courseCommand)
        {
            this.courseQuery = courseQuery;
            this.courseCommand = courseCommand;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            return await courseQuery.GetAllCourses();
        }

        public async Task<CoursesPaginationDTO> GetPageOfCoursesAsync(DashboardTableRequestDTO request)
        {
            var result = await courseQuery.GetGroupOfCourses(request);
            return result;
        }
        public async Task<CourseDTO> CreateNewCourseAsync(CourseDTO course)
        {
            CourseDTO result = await courseCommand.CreateNewCourseAsync(course);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<CourseDTO> DeleteSingleCourseAsync(string Id)
        {
            CourseDTO result = await courseCommand.DeleteSingleCourseAsync(Id);
            return result;
        }

        public async Task<CourseDTO> EditCourseAsync(CourseDTO course)
        {
            CourseDTO result = await courseCommand.EditCourseAsync(course);
            return result;
        }
    }
}

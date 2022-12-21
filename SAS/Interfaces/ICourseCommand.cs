using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public interface ICourseCommand
    {
        public Task<CourseDTO> CreateNewCourseAsync(CourseDTO course);
        public Task<CourseDTO> EditCourseAsync(CourseDTO course);
        public Task<CourseDTO> DeleteSingleCourseAsync(string id);

    }
}

using SAS.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SAS.Interfaces
{
    public interface ICourseService
    {
        public Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        public Task<CoursesPaginationDTO> GetPageOfCoursesAsync(DashboardTableRequestDTO request);
        public Task<CourseDTO> CreateNewCourseAsync(CourseDTO course);

        public Task<CourseDTO> DeleteSingleCourseAsync(string id);

        public Task<CourseDTO> EditCourseAsync(CourseDTO course);
    }
}

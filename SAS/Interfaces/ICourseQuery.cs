using SAS.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public interface ICourseQuery
    {
        public Task<CourseDTO> GetCourseByName(string Name);

        public Task<CourseDTO> GetCourseById(string Id);

        public Task<IEnumerable<CourseDTO>> GetAllCourses();

        public IQueryable<CourseDTO> SortCourses(IQueryable<CourseDTO> courses, DashboardTableRequestDTO request);

        public Task<CoursesPaginationDTO> GetGroupOfCourses(DashboardTableRequestDTO request);
    }
}

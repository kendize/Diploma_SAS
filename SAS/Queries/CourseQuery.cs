using Microsoft.EntityFrameworkCore;
using SAS.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public class CourseQuery : ICourseQuery
    {
        private readonly ApplicationContext db;
        public CourseQuery(ApplicationContext context)
        {
            db = context;
        }

        public async Task<CourseDTO> GetCourseByName(string Name)
        {
            CourseDTO result = await db.Courses.FirstOrDefaultAsync(x => x.CourseName == Name);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<CourseDTO> GetCourseById(string Id)
        {
            CourseDTO result = await db.Courses.FirstOrDefaultAsync(x => x.Id == Id);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses()
        {
            var result = await db.Courses.ToListAsync();
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public IQueryable<CourseDTO> SortCourses(IQueryable<CourseDTO> courses, DashboardTableRequestDTO request)
        {

            switch (request.orderColumnName)
            {
                case "id":
                    return request.orderBy == "ascend" ? courses.OrderBy(model => model.Id) : courses.OrderByDescending(model => model.Id);
                case "courseName":
                    return request.orderBy == "ascend" ? courses.OrderBy(model => model.CourseName) : courses.OrderByDescending(model => model.CourseName);
                default:
                    return courses;
            }
        }

        public async Task<CoursesPaginationDTO> GetGroupOfCourses(DashboardTableRequestDTO request)
        {
            IQueryable<CourseDTO> coursesQuery = db.Courses;
            int numberOfCourses = 0;
            if (!string.IsNullOrEmpty(request.searchString))
            {
                coursesQuery = db.Courses.Where(model => model.CourseName.Contains(request.searchString));
            }

            numberOfCourses = await coursesQuery.CountAsync();
            coursesQuery = SortCourses(coursesQuery, request);
            var pageOfUsers = await coursesQuery.Skip((request.pageNumber - 1) * request.pageSize)
                                              .Take(request.pageSize)
                                              .ToListAsync();

            var result = new CoursesPaginationDTO
            {
                courses = pageOfUsers,
                NumberOfCourses = numberOfCourses,
            };

            return result;
        }
    }
}

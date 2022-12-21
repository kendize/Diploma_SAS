using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAS.DTO;
using SAS.Interfaces;
using System.Threading.Tasks;

namespace SAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetGroupOfCourses(int pageNumber, //is it neccessary to Change into DTO & move default values to Front?
                                                           int pageSize,
                                                           string searchString = "",
                                                           string orderColumnName = "Id",
                                                           string orderBy = "ascend")
        {
            DashboardTableRequestDTO request = new DashboardTableRequestDTO
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                searchString = searchString,
                orderColumnName = orderColumnName,
                orderBy = orderBy
            };

            var result = await courseService.GetPageOfCoursesAsync(request);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CreateNewCourseAsync(CourseDTO course)
        {
            var result = await courseService.CreateNewCourseAsync(course);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // PUT api/course/id
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> EditCourseAsync(CourseDTO course)
        {
            var result = await courseService.EditCourseAsync(course);
            return Ok(result);
        }

        // DELETE api/course/id
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseDTO>> DeleteSingleCourseAsync(string id)
        {
            var result = await courseService.DeleteSingleCourseAsync(id);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}

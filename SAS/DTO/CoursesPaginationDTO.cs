using System.Collections.Generic;

namespace SAS.DTO
{
    public class CoursesPaginationDTO
    {
        public int NumberOfCourses { get; set; }
        public IEnumerable<CourseDTO> courses { get; set; }
    }
}

using System.Collections.Generic;
namespace SAS.DTO
{
    public class CourseDTO
    {
        public string Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImgUrl { get; set; }
        public ICollection<UserCourseDTO> UserCourses { get; set; }

    }
}

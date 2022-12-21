using System.Collections.Generic;

namespace SAS.DTO
{
    public class DashboardTableResponseDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string RegisteredDate { get; set; }
        public ICollection<UserCourseDTO> UserCourses { get; set; }



    }
}

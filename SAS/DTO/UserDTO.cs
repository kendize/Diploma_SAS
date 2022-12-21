
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SAS.DTO
{
    public class UserDTO : IdentityUser
    {
        override public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        override public string Email { get; set; }
        public string RegisteredDate { get; set; }
        public string StudyDate { get; set; }
        public string RefreshToken { get; set; }
        public ICollection<UserCourseDTO> UserCourses { get; set; }



    }
}

using System;
using System.Collections.Generic;
namespace SAS.DTO
{
    public class UserCourseDTO
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public CourseDTO Course { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        //public string HangfireJobsId { get; set; }
        public IEnumerable<HangfireJobDTO> HangfireJobs { get; set; }
        public DateTime StudyDate { get; set; }


    }
}

namespace SAS.DTO
{
    public class HangfireJobDTO
    {
        public string Id { get; set; }
        public string userCourseId { get; set; }
        public UserCourseDTO userCourse { get; set; }

    }
}

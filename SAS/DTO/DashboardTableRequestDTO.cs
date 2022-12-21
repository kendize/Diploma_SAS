namespace SAS.DTO
{
    public class DashboardTableRequestDTO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string searchString { get; set; }
        public string orderColumnName { get; set; }
        public string orderBy { get; set; }

        public string searchColumn { get; set; }
    }
}

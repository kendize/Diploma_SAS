using System.Collections.Generic;

namespace SAS.DTO
{
    public class UsersPaginationDTO
    {
        public int NumberOfUsers { get; set; }
        public IEnumerable<DashboardTableResponseDTO> users { get; set; }
    }
}

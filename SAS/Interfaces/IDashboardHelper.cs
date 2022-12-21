using SAS.DTO;
using System.Linq;


namespace SAS.Interfaces
{
    public interface IDashboardHelper
    {
        public IQueryable<UserDTO> SortUsers(IQueryable<UserDTO> users, DashboardTableRequestDTO request);
        public IQueryable<UserDTO> SearchUsers(IQueryable<UserDTO> users, DashboardTableRequestDTO request);
    }
}

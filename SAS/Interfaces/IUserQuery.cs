using SAS.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public interface IUserQuery
    {
        public Task<string> GetRoleById(string id);
        public Task<UserDTO> GetUserById(string Id);
        public Task<UserDTO> GetUserByEmail(string Email);
        public Task<UserDTO> GetUserByRefreshToken(string refreshToken);
        public int GetTotalNumberOfUsers();
        public Task<UsersPaginationDTO> GetUsers(DashboardTableRequestDTO request);

    }
}

using SAS.DTO;
using System.Threading.Tasks;


namespace SAS.Interfaces
{
    public interface IAdminService
    {

        public Task<UsersPaginationDTO> GetPaginationInfo(DashboardTableRequestDTO request);

        public Task<UserDTO> CreateNewUserAsync(RegistrationDTO user);

        public Task<UserDTO> DeleteSingleUserAsync(string id);

        public Task<UserDTO> EditUserAsync(UserDTO user);


    }
}

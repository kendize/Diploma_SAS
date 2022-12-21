using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using System.Threading.Tasks;

namespace SAS.Services
{
    public class AdminService : IAdminService
    {

        private readonly IUserQuery userQuery;
        private readonly IUserCommand userCommand;

        public AdminService(ApplicationContext context,
                            IUserQuery userQuery,
                            IUserCommand userCommand)
        {
            this.userQuery = userQuery;
            this.userCommand = userCommand;
        }

        public async Task<UsersPaginationDTO> GetPaginationInfo(DashboardTableRequestDTO request)
        {
            return await userQuery.GetUsers(request);
        }

        public async Task<UserDTO> CreateNewUserAsync(RegistrationDTO user)
        {
            UserDTO result = await userCommand.CreateNewUserAsync(user);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<UserDTO> DeleteSingleUserAsync(string Id)
        {
            UserDTO result = await userCommand.DeleteSingleUserAsync(Id);
            return result;
        }

        public async Task<UserDTO> EditUserAsync(UserDTO user)
        {
            UserDTO result = await userCommand.EditUserAsync(user);
            return result;
        }
    }
}

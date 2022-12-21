using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Queries
{
    public interface IUserCommand
    {
        public Task<bool> ConfirmEmail(string userId, string code);
        public Task<UserDTO> CreateNewUserAsync(RegistrationDTO RegistrationUser);

        public Task<UserDTO> CreateNewFacebookUserAsync(FacebookDataDTO user);
        public Task<UserDTO> DeleteSingleUserAsync(string Id);
        public Task<UserDTO> EditUserAsync(UserDTO user);
    }
}

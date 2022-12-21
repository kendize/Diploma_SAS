using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Interfaces
{
    public interface IFacebookAuthService
    {
        public Task<bool> IsAccessTokenValid(string accessToken);
        public Task<FacebookDataDTO> GetFacebookData(string accessToken);
        public Task<AuthenticationResultDTO> AuthenticateFacebook(string acceessToken);
    }
}

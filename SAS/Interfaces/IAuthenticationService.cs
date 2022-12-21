using SAS.DTO;
using System.Threading.Tasks;

namespace SAS.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResultDTO> Authenticate(AuthenticationRequestDTO model);
        public Task<AuthenticationResultDTO> ReAuthenticate(RefreshTokenDTO model);
    }
}

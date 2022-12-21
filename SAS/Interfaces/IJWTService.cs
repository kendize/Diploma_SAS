using SAS.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAS.Interfaces
{
    public interface IJWTService
    {
        public Task<ClaimsIdentity> GetUserClaimsIdentity(UserDTO user);
        public Task<string> GenerateToken(UserDTO user, string TokenType);
    }
}

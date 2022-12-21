using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAS.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly ApplicationContext db;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IUserQuery userQuery;
        private readonly IUserCommand userCommand;
        private readonly IJWTService jWTService;
        private const string TokenValidationUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
        private const string UserInfoUrl = "https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={0}";

        public FacebookAuthService( ApplicationContext context,
                                    IHttpClientFactory httpClientFactory,
                                    IConfiguration configuration,
                                    IUserQuery userQuery,
                                    IUserCommand userCommand,
                                    IJWTService jWTService)

        {
            db = context;
            this.userQuery = userQuery;
            this.userCommand = userCommand;
            this.jWTService = jWTService;
            this.httpClientFactory = httpClientFactory;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<bool> IsAccessTokenValid(string accessToken)
        {
            var formattedUrl = string.Format(TokenValidationUrl,
                                             accessToken,
                                             Configuration["Authentication:Facebook:AppId"],
                                             Configuration["Authentication:Facebook:AppSecret"]);
            var result = await httpClientFactory.CreateClient()
                                                .GetAsync(formattedUrl);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<FacebookDataDTO> GetFacebookData(string accessToken)
        {
            var formattedUrl = string.Format(UserInfoUrl,
                                             accessToken);
            var result = await httpClientFactory.CreateClient()
                                                .GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();
            var stringResponse = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookDataDTO>(stringResponse);
        }

        public async Task<AuthenticationResultDTO> AuthenticateFacebook(string accessToken)
        {
            if (!(await IsAccessTokenValid(accessToken)))
            {
                return null;
            }

            var FacebookData = await GetFacebookData(accessToken);
            UserDTO user = await userQuery.GetUserById(FacebookData.id);
            if (user == null)
            {
                user = await userCommand.CreateNewFacebookUserAsync(FacebookData);
            }

            var userRole = await userQuery.GetRoleById(user.Id);
            user.RefreshToken = await jWTService.GenerateToken(user, "refresh");
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return new AuthenticationResultDTO()
            {
                accessToken = await jWTService.GenerateToken(user, "access"),
                refreshToken = user.RefreshToken,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Age = user.Age
            };
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using System.Threading.Tasks;

namespace SAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authenticationService;
        private readonly SignInManager<UserDTO> signInManager;
        private readonly IFacebookAuthService facebookAuthService;
        private readonly IUserCommand userCommand;

        public AuthenticationController(IAuthenticationService authenticationService,
                                            SignInManager<UserDTO> signInManager,
                                            IFacebookAuthService facebookAuthService,
                                            IUserCommand userCommand)
        {
            this.authenticationService = authenticationService;
            this.signInManager = signInManager;
            this.facebookAuthService = facebookAuthService;
            this.userCommand = userCommand;
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return BadRequest();
        }

        [HttpGet]
        [Route("[action]")]
        // GET api/admin/
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, string code)
        {
            if (!(await userCommand.ConfirmEmail(userId, code)))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AuthenticateFacebook([FromQuery] string accessToken)
        {
            var result = await facebookAuthService.AuthenticateFacebook(accessToken);
            if (result == null)
            {
                return BadRequest();
            }

            return Json(result);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticationRequestDTO model)
        {
            var result = await authenticationService.Authenticate(model);
            if (result == null)
            {
                return BadRequest();
            }

            return Json(result);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> ReAuthenticate(RefreshTokenDTO model) 
        {
            var result = await authenticationService.ReAuthenticate(model);
            if (result == null)
            {
                return BadRequest();
            }

            return Json(result);
        }
    }
}

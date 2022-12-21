using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService subscriptionService;
        private readonly ISubscriptionQuery subscriptionQuery;
        private readonly UserManager<UserDTO> userManager;
        private readonly INotificationService notificationService;

        public SubscriptionController(ISubscriptionService subscriptionService,
                                      UserManager<UserDTO> userManager,
                                      ISubscriptionQuery subscriptionQuery,
                                      INotificationService notificationService)
        {
            this.subscriptionService = subscriptionService;
            this.userManager = userManager;
            this.subscriptionQuery = subscriptionQuery;
            this.notificationService = notificationService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Subscribe(SubscriptionRequestDTO userRequest)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                                      .FirstOrDefault()
                                      .Value;

            var validRequest = new SubscriptionRequestDTO
            {
                UserId = userId,
                CourseId = userRequest.CourseId,
                StudyDate = userRequest.StudyDate
            };

            var result = await subscriptionService.Subscribe(validRequest);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UnSubscribe(SubscriptionRequestDTO userRequest)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                                      .FirstOrDefault()
                                      .Value;

            var validRequest = new SubscriptionRequestDTO
            {
                UserId = userId,
                CourseId = userRequest.CourseId
            };

            var result = await subscriptionService.UnSubscribe(validRequest);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetUserSubscriptions()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                                      .FirstOrDefault()
                                      .Value;

            var result = await subscriptionQuery.GetAllUserSubscriptions(userId);
            return Ok(result);
        }
    }
}

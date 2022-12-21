using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAS.DTO;
using SAS.Interfaces;
using System.Threading.Tasks;

namespace SAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IEmailService emailService;

        public AdminController(IAdminService adminService,
                                IEmailService emailService)
        {
            this.adminService = adminService;
            this.emailService = emailService;
        }

        // GET api/admin/
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetGroupOfUsers(int pageNumber,
                                                        int pageSize,
                                                        string searchString,//isnullorempt
                                                        string searchColumn = "FirstName",
                                                        string orderColumnName = "Id",
                                                        string orderBy = "ascend"
                                                       )
        {
            var request = new DashboardTableRequestDTO //var
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                searchString = searchString,
                orderColumnName = orderColumnName,
                orderBy = orderBy,
                searchColumn = searchColumn,
            };

            var result = await adminService.GetPaginationInfo(request);
            return Ok(result);
        }

        // POST api/admin
        [HttpPost]
        public async Task<ActionResult> CreateNewUserAsync(RegistrationDTO user)
        {
            var result = await adminService.CreateNewUserAsync(user);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // PUT api/admin/id

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult> EditUserAsync(UserDTO user)
        {
            var result = await adminService.EditUserAsync(user);
            return Ok(result);
        }

        // DELETE api/admin/id
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RegistrationDTO>> DeleteSingleUserAsync(string id)
        {
            var result = await adminService.DeleteSingleUserAsync(id);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}

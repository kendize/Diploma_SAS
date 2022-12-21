using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAS.DTO;
using SAS.Helpers;
using SAS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
namespace SAS.Queries
{
    public class UserQuery : IUserQuery
    {
        private readonly ApplicationContext db;
        private readonly UserManager<UserDTO> userManager;
        private readonly IMapper mapper;
        private readonly IDashboardHelper dashboardHelper;
        public UserQuery(ApplicationContext context,
                         UserManager<UserDTO> userManager,
                         IMapper mapper,
                         IDashboardHelper dashboardHelper)
        {
            db = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.dashboardHelper = dashboardHelper;
        }

        public async Task<string> GetRoleById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var role = await userManager.GetRolesAsync(user);
            return role.FirstOrDefault();
        }

        public async Task<UserDTO> GetUserById(string Id)
        {
            UserDTO user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<UserDTO> GetUserByEmail(string Email)
        {
            UserDTO user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<UserDTO> GetUserByRefreshToken(string refreshToken)
        {
            UserDTO user = await db.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public int GetTotalNumberOfUsers()
        {
            return db.Users.Count();
        }

        

        public async Task<UsersPaginationDTO> GetUsers(DashboardTableRequestDTO request)
        {
            IQueryable<UserDTO> usersQuery = db.Users.Include(sc => sc.UserCourses)
                                                     .ThenInclude(c => c.Course);
            int numberOfUsers = 0;

            if (!string.IsNullOrEmpty(request.searchString) && !string.IsNullOrEmpty(request.searchColumn))
            {
                //usersQuery = db.Users.Where(model => model.Email.Contains(request.searchString));
                usersQuery = dashboardHelper.SearchUsers(usersQuery, request);
            }

            numberOfUsers = await usersQuery.CountAsync();
            usersQuery = dashboardHelper.SortUsers(usersQuery, request);
            var pageOfUsers = await usersQuery.Skip((request.pageNumber - 1) * request.pageSize)
                                              .Take(request.pageSize)
                                              .ToListAsync();
            var result = new UsersPaginationDTO
            {
                users = mapper.Map<List<UserDTO>, List<DashboardTableResponseDTO>>(pageOfUsers),
                NumberOfUsers = numberOfUsers,
            };

            return result;
        }
    }
}

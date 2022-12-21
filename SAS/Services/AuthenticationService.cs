using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using SAS.Validators;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace SAS.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJWTService jWTService;
        private readonly IUserQuery userQuery;
        private readonly UserManager<UserDTO> userManager;
        private readonly ApplicationContext db;
        private readonly SignInManager<UserDTO> signInManager;
        public AuthenticationService(IJWTService jWTService,
                                        IUserQuery userQuery,
                                        UserManager<UserDTO> manager,
                                        ApplicationContext context,
                                        SignInManager<UserDTO> signInManager)
        {
            this.jWTService = jWTService;
            this.userQuery = userQuery;
            db = context;
            userManager = manager;
            this.signInManager = signInManager;
        }

        public async Task<AuthenticationResultDTO> Authenticate(AuthenticationRequestDTO model)
        {

            var validator = new AuthenticationRequestDTOValidator();
            ValidationResult validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return null;
            }

            UserDTO user = await userQuery.GetUserByEmail(model.Email);
            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false); // 

            if (result.Succeeded && await userManager.IsEmailConfirmedAsync(user))
            {
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

            return null; 
        }

        public async Task<AuthenticationResultDTO> ReAuthenticate(RefreshTokenDTO model)
        {
            UserDTO user = await userQuery.GetUserByRefreshToken(model.refreshToken);           
            if (user != null)
            {
                JwtSecurityToken refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(model.refreshToken);
                DateTime ExpiredDate = refreshToken.ValidTo;
                if (DateTime.UtcNow > ExpiredDate)
                {
                    return null;
                }

                var userRole = await userQuery.GetRoleById(user.Id);
                user.RefreshToken = await jWTService.GenerateToken(user, "refresh");
                string accessToken = await jWTService.GenerateToken(user, "access");

                db.Users.Update(user);
                await db.SaveChangesAsync();

                return new AuthenticationResultDTO
                {
                    accessToken = accessToken,
                    refreshToken = user.RefreshToken,
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Age = user.Age
                };
            }

            return null;
            
        }


    }
}

using Microsoft.AspNetCore.Identity;
using SAS.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace SAS
{
    public static class SampleData
    {
        public static async Task InitializeAsync(UserManager<UserDTO> userManager, 
                                                 RoleManager<IdentityRole> roleManager, 
                                                 ApplicationContext db)
        {
            string adminEmail = "admin@sas.com";
            string password = "11111";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                UserDTO admin = new UserDTO
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    Age = 21
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.ConfirmEmailAsync(admin, await userManager.GenerateEmailConfirmationTokenAsync(admin));
                }
            }
            if (!db.Courses.Any())
            {
                CourseDTO testCourse = new CourseDTO
                {
                    Id = "1",
                    CourseName = "test 1",
                    CourseDescription = "test course",
                    CourseImgUrl = "https://cdn.pixabay.com/photo/2013/07/12/17/47/test-pattern-152459_960_720.png"
                };
                db.Courses.Add(testCourse);
                db.SaveChanges();
            }
        }
    }
}

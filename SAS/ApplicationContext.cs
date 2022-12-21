using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAS.DTO;
namespace SAS
{
    public class ApplicationContext : IdentityDbContext<UserDTO>
    {
        public DbSet<CourseDTO> Courses { get; set; }
        public DbSet<UserCourseDTO> UserCourses { get; set; }

        public DbSet<HangfireJobDTO> HangfireJob { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserCourseDTO>()
                //.HasKey(sc => new { sc.UserId, sc.CourseId });
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<UserCourseDTO>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserCourses)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserCourseDTO>()
                .HasOne(sc => sc.Course)
                .WithMany(s => s.UserCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<HangfireJobDTO>()
                .HasKey(sc => new { sc.Id, sc.userCourseId });

            modelBuilder.Entity<HangfireJobDTO>()
                .HasOne<UserCourseDTO>(s => s.userCourse)
                .WithMany(g => g.HangfireJobs)
                .HasForeignKey(sc => sc.userCourseId);
        }
    }
}
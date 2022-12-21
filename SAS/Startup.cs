using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SAS.DTO;
using SAS.Helpers;
using SAS.Interfaces;
using SAS.Mappings;
using SAS.Queries;
using SAS.Services;
using System;
using NLog;
using NLog.Extensions.Logging;

namespace SAS
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
                       Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.Configure<MailSettingsDTO>(options => Configuration.GetSection("MailSettings").Bind(options));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));    
            services.AddHangfireServer();                                               
            services.AddCors();
            services.AddHttpClient();
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));
            services.AddIdentity<UserDTO, IdentityRole>(opts =>
            {
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 1;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<ApplicationContext>()
               .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IDashboardHelper, DashboardHelper>();

            services.AddScoped<IUserQuery, UserQuery>();
            services.AddScoped<ICourseQuery, CourseQuery>();
            services.AddScoped<IUserCommand, UserCommand>();
            services.AddScoped<ICourseCommand, CourseCommand>();
            services.AddScoped<IFacebookAuthService, FacebookAuthService>();
            services.AddScoped<ITemplateHelper, TemplateHelper>();
            services.AddScoped<ISubscriptionQuery, SubscriptionQuery>();
            services.AddScoped<ISubscriptionCommand, SubscriptionCommand>();
            services.AddScoped<INotificationService, NotificationService>();

            

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,

                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,

                            ValidateLifetime = true,

                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    })

                    .AddCookie(options =>
                    {
                        options.LoginPath = "/account/facebook-login";
                    })

                    .AddFacebook(options =>
                    {
                        options.AppId = Configuration["Authentication:Facebook:AppId"];
                        options.AppSecret = Configuration["Authentication:Facebook:AppSecret"]; 
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();

            });

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new Hangfire.Dashboard.BasicAuthorization.BasicAuthAuthorizationFilter(new Hangfire.Dashboard.BasicAuthorization.BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users = new []
                        {
                            new Hangfire.Dashboard.BasicAuthorization.BasicAuthAuthorizationUser
                            {
                                Login = Configuration["Authentication:Admin:Email"],
                                PasswordClear = Configuration["Authentication:Admin:Password"]
                            }
                        }

                })
            },

                AppPath = Configuration["AdminSafeList"]
            });
        }
    }
}
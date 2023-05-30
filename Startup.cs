using Angular_WebApi_WithJWT_WithIdentityFramewor.Identity;
using Angular_WebApi_WithJWT_WithIdentityFramewor.ServiceContract;
using Angular_WebApi_WithJWT_WithIdentityFramewor.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDbContext>
            (option =>option.UseSqlServer(Configuration.GetConnectionString("conStr"),
           b => b.MigrationsAssembly("Angular_WebApi_WithJWT_WithIdentityFramewor")));

            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
            services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
            services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IUserService, UserService>();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleManager<ApplicationRoleManager>()
            .AddSignInManager<ApplicationSignInManager>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddDefaultTokenProviders();


            services.AddScoped<ApplicationRoleStore>();
            services.AddScoped<ApplicationUserStore>();

            //Add JWT Authentication

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);

            var appSetting = appSettingSection.Get<AppSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(appSetting.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddCookie()
              .AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });


            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Angular_WebApi_WithJWT_WithIdentityFramewor", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Angular_WebApi_WithJWT_WithIdentityFramewor v1"));
            }

            app.UseAuthentication();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Data
            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.
                GetRequiredService<IServiceScopeFactory>();
            //using (IServiceScope scope = serviceScopeFactory.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService
            //        <RoleManager<ApplicationRole>>();
            //    var userManager = scope.ServiceProvider.GetRequiredService
            //        <UserManager<ApplicationUser>>();
            //    //Create Admin Role
            //    if (!await roleManager.RoleExistsAsync("Admin"))
            //    {
            //        var role = new ApplicationRole();
            //        role.Name = "Admin";
            //        await roleManager.CreateAsync(role);
            //    }


            //    //Create User Role
            //    if (!await roleManager.RoleExistsAsync("User"))
            //    {
            //        var role = new ApplicationRole();
            //        role.Name = "User";
            //        await roleManager.CreateAsync(role);
            //    }


            //    //Create Admin User

            //    if (await userManager.FindByNameAsync("admin") == null)
            //    {
            //        var user = new ApplicationUser();
            //        user.UserName = "admin";
            //        user.Email = "admin@gmail.com";
            //        var userPassword = "Admin@123";
            //        var chkuser = await userManager.CreateAsync(user, userPassword);
            //        if (chkuser.Succeeded)
            //        {
            //            await userManager.AddToRoleAsync(user, "Admin");
            //        }
            //    }

            //    //Create Employee User

            //    if (await userManager.FindByNameAsync("user") == null)
            //    {
            //        var user = new ApplicationUser();
            //        user.UserName = "user";
            //        user.Email = "user@gmail.com";
            //        var userPassword = "Admin@123";
            //        var chkuser = await userManager.CreateAsync(user, userPassword);
            //        if (chkuser.Succeeded)
            //        {
            //            await userManager.AddToRoleAsync(user, "User");
            //        }
            //    }
            //}





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

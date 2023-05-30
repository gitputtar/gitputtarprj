using Angular_WebApi_WithJWT_WithIdentityFramewor.Identity;
using Angular_WebApi_WithJWT_WithIdentityFramewor.ServiceContract;
using Angular_WebApi_WithJWT_WithIdentityFramewor.Utility;
using Angular_WebApi_WithJWT_WithIdentityFramewor.ViewModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly AppSettings _appSetting;
        public UserService(IOptions<AppSettings> appSettings, ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager)
        {
            _applicationSignInManager = applicationSignInManager;
            _applicationUserManager = applicationUserManager;
            _appSetting = appSettings.Value;
        }

        public async Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel)
        {
            var result = await _applicationSignInManager.PasswordSignInAsync
                (loginViewModel.UserName, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync
                    (loginViewModel.UserName);
                applicationUser.PasswordHash = "";
                //  JWT Token
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, SD.Role_Admin))
                    applicationUser.Role = SD.Role_Admin;
               if (await _applicationUserManager.IsInRoleAsync(applicationUser, SD.Role_User))
                   applicationUser.Role = SD.Role_User;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSetting.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                        new Claim(ClaimTypes.Name,applicationUser.Id),
                        new Claim(ClaimTypes.Email,applicationUser.Email),
                        new Claim(ClaimTypes.Role,applicationUser.Role)
                  }),
                    Expires = DateTime.UtcNow.AddHours(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                  SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);

                return applicationUser;
            }
            else
            {
                return null;
            }
        }
    }
}

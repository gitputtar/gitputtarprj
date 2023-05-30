using Angular_WebApi_WithJWT_WithIdentityFramewor.Identity;
using Angular_WebApi_WithJWT_WithIdentityFramewor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor.ServiceContract
{
    public interface IUserService
    {
        Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel);
    }
}

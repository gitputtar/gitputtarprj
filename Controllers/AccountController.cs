using Angular_WebApi_WithJWT_WithIdentityFramewor.Identity;
using Angular_WebApi_WithJWT_WithIdentityFramewor.ServiceContract;
using Angular_WebApi_WithJWT_WithIdentityFramewor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationSignInManager _applicationSignInManager;
        public AccountController(IUserService userService, ApplicationSignInManager applicationSignInManager)
        {
            _userService = userService;
            _applicationSignInManager = applicationSignInManager;
        }
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
        {
            var user = await _userService.Authenticate(loginViewModel);
            if (user == null)
                return BadRequest(new { message = "Wrong UserName or Password" });
            return Ok(user);
        }

    }
}

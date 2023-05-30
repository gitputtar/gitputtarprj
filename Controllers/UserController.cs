using Angular_WebApi_WithJWT_WithIdentityFramewor.Identity;
using Angular_WebApi_WithJWT_WithIdentityFramewor.Models;
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
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetUser()
        {
            var usr = _context.Users.ToList();
            return Ok(usr);
        }
        [HttpPost]
        public IActionResult SaveUser([FromBody] User user)
        {
            if (User == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if (User == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok();

        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var UserInDb = _context.Users.Find(id);
            if (UserInDb == null)
                return NotFound();
            _context.Users.Remove(UserInDb);
            _context.SaveChanges();
            return Ok();

        }
    }
}
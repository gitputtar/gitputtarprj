using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor.Identity
{
    public class ApplicationUserStore: UserStore<ApplicationUser>
    {
        public ApplicationUserStore(ApplicationDbContext context) : base(context)
        {

        }
    }
}

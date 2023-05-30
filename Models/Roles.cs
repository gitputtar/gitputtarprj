using System.ComponentModel.DataAnnotations;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor.Models
{
    public class Roles
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
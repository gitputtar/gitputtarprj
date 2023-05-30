using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_WebApi_WithJWT_WithIdentityFramewor.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string TrIdNo { get; set; }
        [Required]
        public string PassportNo { get; set; }
        //public FormFile image { get; set; }
        //public FormFile image1 { get; set; }

        //public FormFile image2 { get; set; }

        //public FormFile image3 { get; set; }




    }
}

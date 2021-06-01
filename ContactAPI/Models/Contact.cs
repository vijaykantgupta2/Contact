using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAPI.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(70)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        public int Phone { get; set; }

        [Column(TypeName = "bit")]
        public Boolean Status { get; set; }

        public static explicit operator Contact(ObjectResult v)
        {
            throw new NotImplementedException();
        }
    }
}

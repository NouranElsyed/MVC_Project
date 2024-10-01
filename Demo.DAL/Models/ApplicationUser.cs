using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class IdentifyRole:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IAgree { get; set; }
    }
}

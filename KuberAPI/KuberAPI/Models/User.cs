using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Models
{
    public enum UserType
    {
        Passenger,
        Driver
            
    }
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        [Required]
        public UserType UserType { get; set; }

        public DateTime Created { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication11.Model
{
    public class User
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string passwordSalt { get; set; }
        public int roleId { get; set; }
        public DateTime createdAt { get; set; }

        public User() {                                 // Constructor
        }
    }
}

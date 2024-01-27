using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Models
{
    public class User
    {
        public int Id { get; set; } = 0;
        public required string Username { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required Role Role { get; set; } = Role.user;
        public required string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
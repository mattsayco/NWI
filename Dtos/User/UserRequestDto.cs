using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Dtos.User
{
    public class UserRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role {get; set; }
    }
}
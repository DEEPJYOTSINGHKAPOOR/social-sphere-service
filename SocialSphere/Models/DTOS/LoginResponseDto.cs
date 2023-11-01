using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Models.DTOS
{
    public class LoginResponseDto
    {
        public string FullName { get; set; }

        public string UserName { get; set; }
        public string Token{ get; set; }

        public string UserId{ get; set; }
    }
}

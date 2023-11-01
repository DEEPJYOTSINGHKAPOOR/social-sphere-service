using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Models.DTOS
{
    public class ResetPasswordRequestDto
    {
        public string NewPassword { get; set; }

        public string UserId { get; set; }
    }
}

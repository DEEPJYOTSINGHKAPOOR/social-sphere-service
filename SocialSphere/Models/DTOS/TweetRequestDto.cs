using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Models.DTOS
{
    public class TweetRequestDto
    {
        public string TweetText { get; set; }
        public string UserId { get; set; }
    }
}

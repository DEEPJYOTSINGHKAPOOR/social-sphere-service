using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Models.DTOS
{
    public class TweetResponseDto
    {
        public string TweetText { get; set; }
        public string UserId { get; set; }

        public string TweetId{ get; set; }

        public List<string> TweetComments { get; set; }

        public int LikeCount{ get; set; }

        public DateTime TweetTime { get; set; }

    }
}

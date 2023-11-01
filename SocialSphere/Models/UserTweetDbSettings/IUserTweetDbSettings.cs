using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Models.UserTweetDbSettings
{
    public interface IUserTweetDbSettings
    {
        public string UserCollectionName { get; set; }

        public  string TweetCollectionName { get; set; }
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}

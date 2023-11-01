using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TweeApp.Models
{
    public class UserModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = String.Empty;

        [BsonElement("full_name")]
        public string FullName { get; set; }

        [BsonElement("username")]
        public string Username{ get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }


    public class UserTweetModel : UserModel{
        public List<TweetModel> TweetsList { get; set; }
    }
}

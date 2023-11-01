using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Models
{
    public class TweetModel
    {
        [BsonId,BsonElement("_id"),  BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string TweetId{ get; set; }


        [BsonElement("user_id" ), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("tweet_text")]
        public string TweetText{ get; set; }

        [BsonElement("like_count")]
        public int LikeCount { get; set; }


        [BsonElement("tweet_time")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TweetDateTime { get; set; }

        [BsonElement("comments")]
        public List<string> Comments { get; set; }
    }
}

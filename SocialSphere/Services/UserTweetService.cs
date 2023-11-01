using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TweeApp.Constants;
using TweeApp.Models;
using TweeApp.Models.DTOS;
using TweeApp.Models.UserTweetDbSettings;

namespace TweeApp.Services
{
    public class UserTweetService : IUserTweetService
    {
        private readonly IMongoCollection<UserModel> userCollection;
        private readonly IMongoCollection<TweetModel> tweetCollection;
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        public UserTweetService(IUserTweetDbSettings userTweetDbSettings, IMongoClient mongoClient, IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
        {
            var database = mongoClient.GetDatabase(userTweetDbSettings.DatabaseName);
            userCollection = database.GetCollection<UserModel>(userTweetDbSettings.UserCollectionName);
            tweetCollection = database.GetCollection<TweetModel>(userTweetDbSettings.TweetCollectionName);
            _appSettings = appSettings.Value;
            _httpClientFactory = httpClientFactory;
        }


        private async Task<HttpResponseMessage> CreateNewTokenForUser(UserModel userTweet)
        {

            string createTokenUrl = _appSettings.CreateTokenUrl;

            AuthenticationModel authenticationModel = new AuthenticationModel
            {
                UserName = userTweet.Username,
                Password = userTweet.Password
            };

            string json = JsonConvert.SerializeObject(authenticationModel);

            //Needed to setup the body of the request
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");



            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage httpResponseMessage = await client.PostAsync(createTokenUrl, data);

            return httpResponseMessage;
        }
        public async Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto)
        {
            var filter = Builders<UserModel>.Filter.Eq(user => user.Username, loginRequestDto.UserName);
            var alreadyUserExist = userCollection.Find(filter).FirstOrDefault();


            if (alreadyUserExist == null || alreadyUserExist.Password != loginRequestDto.Password)
            {
                throw new Exception(ExceptionConstants.INVALID_CREDENTIALS);
            }
            HttpResponseMessage httpResponseMessage = await CreateNewTokenForUser(alreadyUserExist);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();

                var authenticationModelRes = JsonConvert.DeserializeObject<AuthenticationModel>(jsonString);


                var loginResponseDto = new LoginResponseDto
                {
                    Token = authenticationModelRes.Token,
                    UserId = alreadyUserExist.UserId,
                    UserName = alreadyUserExist.Username,
                    FullName = alreadyUserExist.FullName
                };

                return loginResponseDto;
            }
            else
            {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);

            }



        }



        public async Task<LoginResponseDto> RegisterUser(UserModel userTweet)
        {
            var filter = Builders<UserModel>.Filter.Eq(user => user.Username, userTweet.Username);
            var alreadyUserExist = userCollection.Find(filter).FirstOrDefault();

            if (alreadyUserExist != null)
            {

                throw new Exception(ExceptionConstants.ALREADY_EXISTS_USER);
            }

            userCollection.InsertOne(userTweet);

            HttpResponseMessage httpResponseMessage = await CreateNewTokenForUser(userTweet);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();

                var authenticationModelRes = JsonConvert.DeserializeObject<AuthenticationModel>(jsonString);


                var loginResponseDto = new LoginResponseDto
                {
                    Token = authenticationModelRes.Token,
                    UserId = userTweet.UserId,
                    UserName = userTweet.Username,
                    FullName = userTweet.FullName
                };

                return loginResponseDto;
            }
            else
            {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);

            }

        }



        public async Task<bool> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var filter = Builders<UserModel>.Filter.Eq(user => user.UserId, resetPasswordRequestDto.UserId);

            var alreadyUserExist = userCollection.Find(filter).FirstOrDefault();

            if (alreadyUserExist == null)
            {

                throw new Exception(ExceptionConstants.INVALID_CREDENTIALS);
            }

            var update = Builders<UserModel>.Update.Set(s => s.Password, resetPasswordRequestDto.NewPassword);
            var result = userCollection.UpdateOne(filter, update);

            if (result.IsAcknowledged)
            {
                return true;
            }
            else
            {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);
            }

        }
        List<UserModel> IUserTweetService.GetAllUsersInfo()
        {
            return userCollection.Find(student => true).ToList<UserModel>();
        }

        private bool doesUserExists(string userId)
        {
            var filter = Builders<UserModel>.Filter.Eq(user => user.UserId, userId);
            var alreadyUserExist = userCollection.Find(filter).FirstOrDefault();


            if (alreadyUserExist == null || alreadyUserExist.Username == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private TweetModel doesTweetExist(string tweetId)
        {
            var filter = Builders<TweetModel>.Filter.Eq(tweet=> tweet.TweetId, tweetId);
            var alreadyTExist = tweetCollection.Find(filter).FirstOrDefault();


            if (alreadyTExist == null)
            {
                return null;
            }
            else
            {
                return alreadyTExist;
            }
        }

        public async Task<TweetResponseDto> MakeTweet(TweetRequestDto tweetRequestDto)
        {
            if (!this.doesUserExists(tweetRequestDto.UserId))
            {
                throw new Exception(ExceptionConstants.INVALID_CREDENTIALS);
            }
            //var userTweetModel = userCollection.Aggregate().Lookup<UserModel, TweetModel, UserTweetModel>(tweetCollection, user => user.UserId, tweet => tweet.UserId, uT => uT.TweetsList).ToList();

            TweetModel tweetModel = new TweetModel
            {
                UserId = tweetRequestDto.UserId,
                LikeCount = 0,
                Comments = new List<string>(),
                TweetDateTime = DateTime.Now,
                TweetText = tweetRequestDto.TweetText,

            };
            tweetCollection.InsertOne(tweetModel);
            TweetResponseDto tweetResponseDto = new TweetResponseDto
            {
                UserId = tweetRequestDto.UserId,
                LikeCount = 0,
                TweetComments = new List<string>(),
                TweetTime = DateTime.Now,
                TweetText = tweetRequestDto.TweetText,
                TweetId = tweetModel.TweetId

            };

            return tweetResponseDto;
        }

        public Task<List<TweetResponseDto>> GetTweetsBySpecificUser(string userId)
        {
            return Task.Run(() => {
                if (!this.doesUserExists(userId))
                {
                    throw new Exception(ExceptionConstants.INVALID_CREDENTIALS);
                }
                var userTweetModelList = userCollection.Aggregate().Lookup<UserModel, TweetModel, UserTweetModel>(tweetCollection, user => user.UserId, tweet => tweet.UserId, uT => uT.TweetsList).ToList();

                List<TweetModel> tweetList = new List<TweetModel>();
                foreach (var userTweetModel in userTweetModelList)
                {
                    if (userTweetModel.UserId == userId)
                    {
                        tweetList = userTweetModel.TweetsList;
                        break;
                    }
                }
                List<TweetResponseDto> tweetResponseDtoList = new List<TweetResponseDto>();



                tweetList.ForEach(tweetModel => tweetResponseDtoList.Add(new TweetResponseDto
                {
                    LikeCount = tweetModel.LikeCount,
                    TweetComments = tweetModel.Comments,
                    TweetId = tweetModel.TweetId,
                    TweetText = tweetModel.TweetText,
                    TweetTime = tweetModel.TweetDateTime,
                    UserId = tweetModel.UserId,
                }));


                return tweetResponseDtoList;
            });


        }

        public Task<bool> LikeTweet(string tweetId)
        {
            TweetModel tweetModel = null;

            if ((tweetModel = this.doesTweetExist(tweetId)) != null) {
                throw new Exception(ExceptionConstants.NO_CONTENT);
            }
            var filter = Builders<TweetModel>.Filter.Eq(user => user.TweetId, tweetId);
            var update = Builders<TweetModel>.Update.Set(s => s.LikeCount, tweetModel.LikeCount + 1);
            var result = tweetCollection.UpdateOne(filter, update);

            if (result.IsAcknowledged)
            {
                return Task.FromResult(true);
            }
            else {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);
            }

        }

        public Task<bool> CommentOnTweet(string tweetId, string commentText)
        {
            TweetModel tweetModel = this.doesTweetExist(tweetId);

            if (tweetModel == null)
            {
                throw new Exception(ExceptionConstants.NO_CONTENT);
            }
            var filter = Builders<TweetModel>.Filter.Eq(user => user.TweetId, tweetId);
            tweetModel.Comments.Add(commentText);
            UpdateDefinition<TweetModel> update = Builders<TweetModel>.Update.Set(s => s.Comments, tweetModel.Comments);
            var result = tweetCollection.UpdateOne(filter, update);

            if (result.IsAcknowledged)
            {
                return Task.FromResult(true);
            }
            else
            {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);
            }


        }

        public Task<bool> EditTweetText(string tweetId, string tweetText)
        {
            TweetModel tweetModel = null;

            if ((tweetModel = this.doesTweetExist(tweetId)) != null)
            {
                throw new Exception(ExceptionConstants.NO_CONTENT);
            }
            var filter = Builders<TweetModel>.Filter.Eq(user => user.TweetId, tweetId);
            
            UpdateDefinition<TweetModel> update = Builders<TweetModel>.Update.Set(s => s.TweetText, tweetText);
            var result = tweetCollection.UpdateOne(filter, update);

            if (result.IsAcknowledged)
            {
                return Task.FromResult(true);
            }
            else
            {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);
            }
        }

        public async Task<bool> DeleteTweet(string tweetId)
        {
            TweetModel tweetModel =this.doesTweetExist(tweetId);

            if ((tweetModel) == null)
            {
                throw new Exception(ExceptionConstants.NO_CONTENT);
            }
            var filter = Builders<TweetModel>.Filter.Eq(user => user.TweetId, tweetId);

           
            var result = await tweetCollection.DeleteOneAsync(filter);

            if (result.IsAcknowledged)
            {
                return true;
            }
            else
            {
                throw new Exception(ExceptionConstants.SOMETHING_WENT_WRONG);
            }
        }
    }
}

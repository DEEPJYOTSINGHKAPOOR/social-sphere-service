using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweeApp.Models;
using TweeApp.Models.DTOS;

namespace TweeApp.Services
{
    public interface IUserTweetService
    {
        public List<UserModel> GetAllUsersInfo();

        public Task<LoginResponseDto>  RegisterUser(UserModel authenticationModel);


        public Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto);



        public Task<bool> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto);


        public Task<TweetResponseDto> MakeTweet(TweetRequestDto tweetRequestDto);

        public Task<List<TweetResponseDto>> GetTweetsBySpecificUser(string userId);


        public Task<bool> LikeTweet(string tweetId);


        public Task<bool> CommentOnTweet(string tweetId, string commentText);

        public Task<bool> EditTweetText(string tweetId, string tweetText);

        public Task<bool> DeleteTweet(string tweetId);
        
    }
}

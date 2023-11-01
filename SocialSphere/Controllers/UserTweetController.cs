using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TweeApp.Models;
using TweeApp.Models.DTOS;
using TweeApp.Services;
using TweeApp.Constants;
using TweeApp.Utils.ExceptionHandler;
namespace TweeApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("AllowOrigin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest )]
    public class UserTweetController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private IUserTweetService _userTweetService;
        private IExceptionHander _exceptionHander;
        public UserTweetController(IAuthenticationService authenticationService, IUserTweetService userTweetService, IExceptionHander exceptionHandler)
        {
            _authenticationService = authenticationService;
            _userTweetService = userTweetService;
            _exceptionHander = exceptionHandler;

        }


        

        /// <summary>
        /// Check Authorized or not.
        /// </summary>
        /// <returns></returns>
        [HttpPost("CheckTokenValidity")]
        [Authorize]
        public IActionResult CheckTokenValidity()
        {
            return Ok();
        }


        /// <summary>
        /// will return token
        /// </summary>
        /// <param name="model">Authentication Model-contains username and password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("CreateToken")]
        //[Route("~/api/Users/Authenticate")]
        public IActionResult CreateToken([FromBody] AuthenticationModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            try {
                var user = _authenticationService.AuthenticateAndCreateToken(model.UserName, model.Password);
                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                return Ok(user);
            }catch(Exception ex)
            {
                return _exceptionHander.handleException(ex);
            }
            
        }


        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null  || loginRequestDto.UserName== null || loginRequestDto.Password == null)
            {
                return BadRequest();
            }
            try
            {
                var list = await _userTweetService.LoginUser(loginRequestDto);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return _exceptionHander.handleException(ex);
            }


        }



        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserModel userTweet)
        {
            if (userTweet == null || userTweet.FullName == null || userTweet.Username == null || userTweet.Password ==null) {
                return BadRequest();
            }
            try {
                var list = await _userTweetService.RegisterUser(userTweet);
                return Ok(list);
            }
            catch(Exception ex)
            {
                return _exceptionHander.handleException(ex);
            }
        }



        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (resetPasswordRequestDto == null || resetPasswordRequestDto.NewPassword == null)
            {
                throw new Exception(ExceptionConstants.BAD_REQUEST);
            }
            try
            {
                var val = await _userTweetService.ResetPassword(resetPasswordRequestDto);
                return Ok(val);
            }
            catch (Exception ex)
            {
                return _exceptionHander.handleException(ex);
            }
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(200)]

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers([FromHeader] string authorization)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.GetAllUsersInfo();
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }

        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpPost("MakeTweet")]
        public async Task<IActionResult> MakeTweet([FromHeader] string authorization,[FromBody] TweetRequestDto tweetRequestDto)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.MakeTweet(tweetRequestDto);
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }



        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpGet("GetTweetBySpecificUser")]
        public async Task<IActionResult> GetTweetBySpecificUser([FromHeader] string authorization,  string userId)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.GetTweetsBySpecificUser(userId);
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }



        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpPatch("LikeTweet")]
        public async Task<IActionResult> LikeTweet([FromHeader] string authorization, string tweetId)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.LikeTweet(tweetId);
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }



        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpPatch("CommentOnTweet")]
        public async Task<IActionResult> CommentOnTweet([FromHeader] string authorization, string tweetId, string commentText)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.CommentOnTweet(tweetId, commentText);
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }


        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpPatch("EditTweetText")]
        public async Task<IActionResult> EditTweetText([FromHeader] string authorization, string tweetId, string tweetText)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.EditTweetText(tweetId, tweetText);
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }



        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpDelete("DeleteTweet")]
        public async Task<IActionResult> DeleteTweet([FromHeader] string authorization, string tweetId)
        {
            try
            {
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var result = await _authenticationService.CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                    if (result != null && result.StatusCode != HttpStatusCode.OK)
                    {
                        return Unauthorized("Authorization Failed! Might be due to invalid token!");
                    }
                    else if (result == null)
                    {
                        return Unauthorized("Please provide authorization token!");
                    }
                }
                else if (authorization == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
                var list = _userTweetService.DeleteTweet(tweetId);
                return Ok(list);
            }
            catch (Exception ex)
            {

                return _exceptionHander.handleException(ex);
            }

        }
    }
}

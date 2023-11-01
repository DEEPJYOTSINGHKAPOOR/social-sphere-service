using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweeApp.Models;

namespace TweeApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
        {
            _appSettings = appSettings.Value;
            _httpClientFactory = httpClientFactory;
        }
        public AuthenticationModel AuthenticateAndCreateToken(string userName, string password)
        {


            AuthenticationModel user = new AuthenticationModel()
            {
                UserName = userName,
                Password = password
            };
            if (user == null)
            {
                return null;
            }
            else
            {
                //we will generte a Jwt token.

                var tokenHandler = new JwtSecurityTokenHandler();
                
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    
                    Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    
                }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    
                    SigningCredentials = new SigningCredentials
                                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);


                user.Password = "";
                return user;
            }
        }

   

  
        public async Task<HttpResponseMessage> CheckTokenValidity(string scheme, string token)
        {
            string authUrl = _appSettings.AuthUrl;

            if (token != null && token.Length != 0)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Post, authUrl);
                HttpResponseMessage response = await client.SendAsync(request);

                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }
        }
    }
}
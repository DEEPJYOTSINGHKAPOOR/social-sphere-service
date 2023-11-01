using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TweeApp.Models;

namespace TweeApp.Services
{
    public interface IAuthenticationService
    {

        public AuthenticationModel AuthenticateAndCreateToken(string userName, string password);

        public  Task<HttpResponseMessage> CheckTokenValidity(string scheme, string token);


    }
}

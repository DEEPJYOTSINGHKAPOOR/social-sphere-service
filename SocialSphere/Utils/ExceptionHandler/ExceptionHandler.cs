using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TweeApp.Constants;

namespace TweeApp.Utils.ExceptionHandler
{
    class ExceptionHandler : IExceptionHander
    {
        public IActionResult handleException(Exception ex)
        {
            if (ex.Message == ExceptionConstants.INVALID_CREDENTIALS)
            {
                return new ContentResult() { Content = ExceptionConstants.INVALID_CREDENTIALS, StatusCode = (int)HttpStatusCode.Unauthorized };
            }
            else if (ex.Message == ExceptionConstants.NO_INTERNET)
            {
                return new ContentResult() { Content = ExceptionConstants.NO_INTERNET, StatusCode = (int)HttpStatusCode.BadGateway };
            }
            else if (ex.Message == ExceptionConstants.ALREADY_EXISTS_USER)
            {
                return new ContentResult() { Content = ExceptionConstants.ALREADY_EXISTS_USER, StatusCode = (int)HttpStatusCode.Conflict };

            }
            else
            {
                return new ContentResult() { Content = ExceptionConstants.SOMETHING_WENT_WRONG + " "+ ex.Message,  StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}

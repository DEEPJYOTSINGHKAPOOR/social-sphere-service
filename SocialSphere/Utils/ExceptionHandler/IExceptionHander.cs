using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp.Utils.ExceptionHandler
{
    public interface IExceptionHander
    {
        public IActionResult handleException(Exception ex);
    }
}

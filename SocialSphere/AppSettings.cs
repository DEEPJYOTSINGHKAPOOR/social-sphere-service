﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweeApp
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string AuthUrl { get; set; }

        public string CreateTokenUrl { get; set; }
    }
}

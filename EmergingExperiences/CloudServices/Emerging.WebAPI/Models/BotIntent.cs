﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emerging.WebAPI.Models
{
    public class BotIntent
    {
        public string Intent { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public string HostName { get; set; }
        public bool IsLuis { get; set; }
    }
}

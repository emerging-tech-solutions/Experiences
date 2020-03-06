using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emerging.WebAPI.Models
{
    public class BotResponse
    { 
        public string TextResponse { get; set; }
        public string Confidence { get; set; }
        public string Base64Audio { get; set; }
    }
}

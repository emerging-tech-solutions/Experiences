using System;
using System.Collections.Generic;
using System.Text;

namespace SharedServices.Models
{
    public class BotResponse
    {
        public string Intent { get; set; }
        public string TextResponse { get; set; }
        public string Confidence { get; set; }
        public string Base64Audio { get; set; }
    }
}

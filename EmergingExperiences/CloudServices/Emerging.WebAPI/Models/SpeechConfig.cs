using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emerging.WebAPI.Models
{
    public class SpeechConfig
    {
        public string AppName { get; set; }
        public string AudioFormat { get; set; }
        public string AzureRegion { get; set; }
        public string AzureTokenUri { get; set; }
        public string AzureEndPointUri { get; set; }
        public string APIKey { get; set; }
    }
}

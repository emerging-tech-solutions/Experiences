using System;
using System.Collections.Generic;
using System.Text;

namespace SharedServices.Models
{
    public class SpeechSynthesisResult
    {
        public string RecognitionStatus { get; set; }
        public string DisplayText { get; set; }
        public int Offset { get; set; }
        public int Duration { get; set; }
    }
}

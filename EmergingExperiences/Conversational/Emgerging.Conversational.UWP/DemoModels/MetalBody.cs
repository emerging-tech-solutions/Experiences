﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emerging.Conversational.UWP
{

    public class Datum
    {
        public string BodyPlainText { get; set; }
        public string BodyHTML { get; set; }
        public string SenderName { get; set; }
        public string ConversationId { get; set; }
        public string Subject { get; set; }
        public string SentOn { get; set; }
    }
    public class MetalBody
    {
        public List<Datum> data { get; set; }
        public MetalBody(string message)
        {
            data = new List<Datum>();
            data.Add(new Datum {
                BodyPlainText = "Hi Mike, I schedule 10s need 304 t6 ss sht cf pi 1 14\\'\\' x 48x96 po qt. ",
                BodyHTML = "<HTML></HTML>",
                SenderName ="test@test.com",
                ConversationId = "22342344444",
                Subject = "quote",
                SentOn = "2019-01-01"
            });
        }
    }
}
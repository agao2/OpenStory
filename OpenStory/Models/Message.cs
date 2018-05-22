using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenStory.Models
{
    public class Message
    {
        public int Id { get; set; }
        public ApplicationUser From { get; set; }
        
        public ApplicationUser To { get; set; }
        
        public DateTime SentTime { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool Read { get; set; }
    }
}
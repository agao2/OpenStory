using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenStory.Models
{
    public class StoryListViewModel
    {
        public IEnumerable<Topic> Stories { get; set; }
    }

    public class TopicPartialViewModel
    {
        public Topic Story { get; set; }
    }

}
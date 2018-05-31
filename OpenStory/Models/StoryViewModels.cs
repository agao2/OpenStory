using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenStory.Models
{
    public class StoryListViewModel
    {
        public IEnumerable<Topic> Stories { get; set; }

        public string SearchString { get; set; }
    }

    public class StoryListPartialViewModel
    {
        public Topic Story { get; set; }
    }

    public class StoryFormViewModel
    {
        public Topic NewTopic { get; set; }

        public Reply TopicReply { get; set; }
    }

    public class TopicViewModel
    {
        public Topic Topic { get; set; }

        public IEnumerable<Reply> Replies { get; set; }

        public Reply NewReply {get;set;}

        public string Username { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

    public class ReplyPartialViewModel
    {
        public Reply Reply { get; set; }

        public int TopicId { get; set; }

        public string Username { get; set; }
    }
}
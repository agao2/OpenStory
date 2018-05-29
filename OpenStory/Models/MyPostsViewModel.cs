using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenStory.Models
{
    public class MyPostsViewModel
    {
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<Reply> Replies { get; set; }
    }

    public class PostPartialViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime PostDate { get; set; }
    }
}
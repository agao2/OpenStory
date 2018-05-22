using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpenStory.Models
{
    public class Topic
    {
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Dislikes { get; set; }
        public int Likes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpenStory.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime PostDate { get; set; }

        [Required( ErrorMessage ="Please Enter in a Title")]
        public string Title { get; set; }

        [Required( ErrorMessage ="Please Enter in some Text")]
        public string Content { get; set; }
        public int Dislikes { get; set; }
        public int Likes { get; set; }
    }
}
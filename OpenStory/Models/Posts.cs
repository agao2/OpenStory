using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace OpenStory.Models
{
    public class Posts
    {
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string Content { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [StringLength(128)]
        public String ApplicationUserId { get; set; }
    } 
}
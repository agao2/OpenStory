using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpenStory.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        
        public DateTime ReplyDate { get; set; }

        public Topic Topic { get; set; }


        [Required(ErrorMessage = "Cannot be blank")]
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenStory.Models
{
    public class Bookmark
    {

        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [StringLength(128)]
        public string ApplicationUserId { get; set; }
        
        public Topic Topic { get; set; }
    }
}
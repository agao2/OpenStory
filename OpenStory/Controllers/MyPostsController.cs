using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenStory.Models;
using System.Data.Entity;

namespace OpenStory.Controllers
{
    public class MyPostsController : Controller
    {

        private ApplicationDbContext _context;

        public MyPostsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            IEnumerable<Topic> topics = from topic in _context.Topics.Include(t => t.ApplicationUser)
                         where topic.ApplicationUser.Id == userId
                         select topic;
            IEnumerable<Reply> replies = from reply in _context.Replies.Include(r => r.ApplicationUser).Include(r => r.Topic)
                                         where reply.ApplicationUser.Id == userId
                                         select reply;


            MyPostsViewModel viewModel = new MyPostsViewModel()
            {
                Topics = topics,
                Replies = replies
            };

            return View(viewModel);
        }
    }
}
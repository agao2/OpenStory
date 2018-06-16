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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context.Dispose();
        }

        [Authorize]
        [Route("MyPosts/{page?}")]
        public ActionResult Index(int? page)
        {
            string userId = User.Identity.GetUserId();

            int fetch = 10;
            if (!page.HasValue)
                page = 1;
            int offset = (page.Value - 1) * fetch;
            int totalReplies = _context.Replies.Include(r => r.ApplicationUser).Where(r => r.ApplicationUser.Id == userId).Count();

            IEnumerable<Reply> replies =  _context.Replies.Include(r => r.ApplicationUser).Include(r => r.Topic)
                                         .Where(r => r.ApplicationUser.Id == userId)
                                         .OrderByDescending(r => r.ReplyDate)
                                         .Skip(() => offset)
                                         .Take(() => fetch)
                                         .ToList();

            int pageCount = (totalReplies / fetch) + 1;

            MyPostsViewModel viewModel = new MyPostsViewModel()
            {
                Replies = replies,
                TotalPages = pageCount,
                Page = page.Value,
            };

            return View(viewModel);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenStory.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OpenStory.Controllers
{
    public class StoriesController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser>  _userManager;
        public StoriesController()
        {
            this._context = new ApplicationDbContext();
            this._userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this._context));
        }

 
        public ActionResult Index()
        {
            var stories = _context.Topics.Include(s => s.ApplicationUser).ToList();
            StoryListViewModel viewModel = new StoryListViewModel()
            {
                Stories = stories
            };
            return View("StoryList" , viewModel);
        }

        public ActionResult New()
        {
            StoryFormViewModel viewModel = new StoryFormViewModel()
            {
                newTopic = new Topic()
            };
            return View("StoryForm", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Topic newTopic)
        {
            if (!ModelState.IsValid)
            {
                StoryFormViewModel viewModel = new StoryFormViewModel()
                {
                    newTopic = newTopic
                };
                return View("StoryForm", viewModel);
            }
            else
            {
                newTopic.PostDate = DateTime.Now;
                newTopic.ApplicationUser = _userManager.FindById(User.Identity.GetUserId());
                newTopic.Likes = 0;
                newTopic.Dislikes = 0;

                _context.Topics.Add(newTopic);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [Route("Stories/Topic/{id}")]
        public ActionResult Topic(int id)
        {
            Topic topic = _context.Topics.Include(s => s.ApplicationUser).Single(t => t.Id == id);
            IEnumerable<Reply> replies = from reply in _context.Replies.Include(s => s.ApplicationUser)
                                         where reply.Topic.Id == topic.Id
                                         select reply;

            String username = "Login to Reply!";
            if (User.Identity.IsAuthenticated)
            {
                username = _userManager.FindById(User.Identity.GetUserId()).Name;
            }

            TopicViewModel viewModel = new TopicViewModel()
            {
                Topic = topic,
                Replies = replies,
                NewReply = new Reply(),
                Username = username
            };
            return View("Topic",viewModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(ReplyPartialViewModel NewReply)
        {

            Topic topic = _context.Topics.Single(t => t.Id == NewReply.TopicId);
            Reply reply = new Reply()
            {
                Topic = topic,
                ApplicationUser = _userManager.FindById(User.Identity.GetUserId()),
                Content = NewReply.Reply.Content,
                ReplyDate = DateTime.Now,
                Likes = 0,
                Dislikes = 0
            };

            _context.Replies.Add(reply);
            _context.SaveChanges();


            return RedirectToAction("Topic", new { id = NewReply.TopicId });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenStory.Models;
using System.Data.Entity;
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

        // Overloaded constructor for dependency injection to make unit testing easier!
        public StoriesController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context.Dispose();
            _userManager.Dispose();
        }

        [Route("Stories/{page?}")]
        public ActionResult Index(int? page)
        {
            int fetch = 10;
            if (!page.HasValue)
                page = 1;
            int offset = (page.Value - 1) * fetch;

            int totalStories = _context.Topics.Count();

            var stories = _context.Topics
                .Include(s => s.ApplicationUser)
                .OrderByDescending(s => s.PostDate)
                .Skip(() => offset)
                .Take(() => fetch)
                .ToList();

            int pageCount = (totalStories / fetch) + 1;

            StoryListViewModel viewModel = new StoryListViewModel()
            {
                Stories = stories,
                Page = page.Value,
                TotalPages = pageCount
            };
            return View("StoryList" , viewModel);
        }

        [Route("Stories/New")]
        public ActionResult New()
        {
            StoryFormViewModel viewModel = new StoryFormViewModel()
            {
                NewTopic = new Topic(),
                TopicReply = new Reply()
            };
            return View("StoryForm", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Stories/Save")]
        public ActionResult Save(Topic newTopic , Reply topicReply)
        {
            if (!ModelState.IsValid)
            {
                StoryFormViewModel viewModel = new StoryFormViewModel()
                {
                    NewTopic = newTopic,
                    TopicReply = topicReply
                    
                };
                return View("StoryForm", viewModel);
            }
            else
            {
                newTopic.PostDate = DateTime.Now;
                ApplicationUser author = _userManager.FindById(User.Identity.GetUserId());
                newTopic.ApplicationUser = author;

                topicReply.ApplicationUser = author;
                topicReply.Topic = newTopic;
                topicReply.ReplyDate = DateTime.Now;
                topicReply.Likes = 0;


                _context.Topics.Add(newTopic);
                _context.Replies.Add(topicReply);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [Route("Stories/Topic/{id}/{page?}")]
        public ActionResult Topic(int id , int? page)
        {
            int fetch = 10;
            if (!page.HasValue)
                page = 1;
            
            int offset = (page.Value-1) * fetch;

            Topic topic = _context.Topics.Include(s => s.ApplicationUser).Single(t => t.Id == id);

            int totalReplies = _context.Replies.Where(r => r.Topic.Id == topic.Id).Count();
           
            IEnumerable<Reply> replies = _context.Replies.Include(s => s.ApplicationUser)
                                         .Where(r => r.Topic.Id == topic.Id)
                                         .OrderBy(r => r.ReplyDate)
                                         .Skip(() => offset)
                                         .Take(() => fetch)
                                         .ToList();

            int pageCount = (totalReplies / fetch) +1;

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
                Username = username,
                Page = page.Value,
                TotalPages = pageCount
            };
            return View("Topic",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Stories/Reply")]
        public ActionResult Reply(ReplyPartialViewModel NewReply)
        {

            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToActionPermanent("Login", "Account", new { returnUrl = "/Stories/Topic/" + NewReply.TopicId });
            }

            Topic topic = _context.Topics.Single(t => t.Id == NewReply.TopicId);

            Reply reply = new Reply()
            {
                Topic = topic,
                ApplicationUser = _userManager.FindById(User.Identity.GetUserId()),
                Content = NewReply.Reply.Content,
                ReplyDate = DateTime.Now,
                Likes = 0
            };
            
            _context.Replies.Add(reply);
            _context.SaveChanges();

            return RedirectToAction("Topic", new { id = NewReply.TopicId });
        }

        [Route("Stories/Search/{query?}/{page?}")]
        public ActionResult Search(string query, int? page)
        {
            int fetch = 10;
            if (!page.HasValue)
                page = 1;
            int offset = (page.Value - 1) * fetch;

            int count = _context.Topics
            .Include(t => t.ApplicationUser)
            .Where(t =>
            t.ApplicationUser.Name.Contains(query) ||
            t.Title.Contains(query)).Count();

            var stories = _context.Topics
                .Include(t => t.ApplicationUser)
                .Where(t =>
                t.ApplicationUser.Name.Contains(query) ||
                t.Title.Contains(query))
                .OrderByDescending(t => t.PostDate)
                .Skip(() => offset)
                .Take(() => fetch);

            int pageCount = (count / fetch) + 1;

            StoryListSearchViewModel viewModel = new StoryListSearchViewModel()
            {
                Stories = stories,
                SearchString = query,
                Page = page.Value,
                TotalPages = pageCount,
            };            
            return View("StoryListSearch", viewModel);
        }
    }
}

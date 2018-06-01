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
            var stories = _context.Topics
                .Include(s => s.ApplicationUser)
                .OrderByDescending(s => s.PostDate)
                .ToList();
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
                NewTopic = new Topic(),
                TopicReply = new Reply()
            };
            return View("StoryForm", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
                topicReply.Dislikes = 0;

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
            
            int offset = (page.Value-1) * 10;

            Topic topic = _context.Topics.Include(s => s.ApplicationUser).Single(t => t.Id == id);

            int totalReplies = _context.Replies.Where(r => r.Topic.Id == topic.Id).Count();
           
            IEnumerable<Reply> replies = _context.Replies.Include(s => s.ApplicationUser)
                                         .OrderBy(r => r.ReplyDate)
                                         .Skip(() => offset)
                                         .Take(() => fetch)
                                         .Where(r => r.Topic.Id == topic.Id).ToList();

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

        [HttpPost]
        public ActionResult Search(StoryListViewModel Search)
        {

            var stories = _context.Topics
                .Include(t => t.ApplicationUser)
                .Where(t =>
                t.ApplicationUser.Name.Contains(Search.SearchString) ||
                t.Title.Contains(Search.SearchString));

            StoryListViewModel viewModel = new StoryListViewModel()
            {
                Stories = stories
            };
            return View("StoryList", viewModel);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenStory.Models;
using System.Data.Entity;

namespace OpenStory.Controllers
{
    public class StoriesController : Controller
    {
        private ApplicationDbContext _context;

        public StoriesController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("Stories")]
        public ActionResult Index()
        {
            var stories = _context.Topics.Include(s => s.ApplicationUser).ToList();
            var viewModel = new StoryListViewModel()
            {
                Stories = stories
            };
            return View("StoryList" , viewModel);
        }
    }
}
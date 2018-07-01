using NUnit.Framework;
using Moq;
using OpenStory.Controllers;
using OpenStory.Models;
using System.Web.Mvc;
using System.Security.Principal;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Security.Claims;
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace OpenStory.UnitTests.Controllers
{
    // All test method names have the following format: [methodName] _[scenario]_[expectedBehavior]
    // Note that it seems the first test actually takes upward of a few second to run, perhaps this is because
    // it is setting up the database connection as the action methods include database access
    [TestFixture]
    class StoriesControllerTests
    {
        [Test]
        public void Index_NullParameter_CorrectView()
        {
            StoriesController controller = new StoriesController();
            ViewResult result = controller.Index(null) as ViewResult;

            Assert.AreEqual("StoryList", result.ViewName);
        }

        [Test]
        public void Index_PageParameter_IsEqual()
        {
            StoriesController controller = new StoriesController();
            int page = 1;
            ViewResult result = controller.Index(page) as ViewResult;
            StoryListViewModel viewModel = result.Model as StoryListViewModel;

            Assert.AreEqual(page, viewModel.Page);
        }

        [Test]
        public void Index_NullParameter_PageEqualsDefaultPage()
        {
            StoriesController controller = new StoriesController();
            ViewResult result = controller.Index(null) as ViewResult;
            StoryListViewModel viewModel = result.Model as StoryListViewModel;
            int defaultPage = 1;

            Assert.AreEqual(defaultPage, viewModel.Page);
        }

        [Test]
        public void New_DefaultScenario_CorrectView()
        {
            StoriesController controller = new StoriesController();
            ViewResult result = controller.New() as ViewResult;

            Assert.AreEqual("StoryForm", result.ViewName);
        }

        [Test]
        public void New_DefaultScenario_NewTopicNotNull()
        {
            StoriesController controller = new StoriesController();
            ViewResult result = controller.New() as ViewResult;

            StoryFormViewModel viewModel = result.Model as StoryFormViewModel;

            Assert.NotNull(viewModel.NewTopic);
        }

        [Test]
        public void New_DefaultScenario_TopicReplyNotNull()
        {
            StoriesController controller = new StoriesController();
            ViewResult result = controller.New() as ViewResult;

            StoryFormViewModel viewModel = result.Model as StoryFormViewModel;

            Assert.NotNull(viewModel.TopicReply);
        }

        [Test]
        public void Save_InvalidModelState_ExpectStoryForm()
        {
            StoriesController controller = new StoriesController();
            controller.ModelState.AddModelError("", "error");
 
            ViewResult result = controller.Save(null,null) as ViewResult;

            Assert.AreEqual("StoryForm", result.ViewName);
        }

        [Test]
        public void SaveValidModelState_ExpectRedirectIndex()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            // setup mock UserManager with fake user
            ApplicationUser testUser = new ApplicationUser()
            {
                Name = "Test",
                Id = "1"
            };
            _userManager.Create(testUser, "fake password");

            //setup mock user and identity for context
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            user.Setup(u => u.Identity).Returns(identity.Object);
            identity.Setup(i => i.Name).Returns("Test");
            identity.Setup(i => i.IsAuthenticated).Returns(true);

            // setup mock context for controller
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.User).Returns(user.Object);
            

            //create controller and set the controller context
            StoriesController controller = new StoriesController(_context, _userManager);
            ControllerContext controllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            controller.ControllerContext = controllerContext;

            // create parameters for the action
            Topic newTopic = new Topic() { Title = "Test Title" };
            Reply topicReply = new Reply() { Content = "Test Content" };

            RedirectToRouteResult result = controller.Save(newTopic, topicReply) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);

            //clean up
            _context.Topics.Remove(newTopic);
            _context.Replies.Remove(topicReply);
            _context.SaveChanges();
            _userManager.Dispose();
            _context.Dispose();
        }
    }
}

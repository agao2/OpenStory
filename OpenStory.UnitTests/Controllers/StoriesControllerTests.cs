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
        
            // Setup mock DB Context
            var mockSetTopics = new Mock<DbSet<Topic>>();
            var mockSetReplies = new Mock<DbSet<Reply>>();
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(m => m.Topics).Returns(mockSetTopics.Object);
            mockContext.Setup(m => m.Replies).Returns(mockSetReplies.Object);

            // Setup usermanager so it will always find 1 user
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var _userManager = new UserManager<ApplicationUser>(userStore.Object);
            ApplicationUser testUser = new ApplicationUser()
            {
                Name = "Test",
                Id = "1"
            };
            userStore.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(testUser);

            //create controller, set the userid so it matches the 1 user we have in user manager
            StoriesController controller = new StoriesController(mockContext.Object, _userManager)
            {
                GetUserId = () => "1"
            };


            // create parameters for the action
            Topic newTopic = new Topic() { Title = "Test Title" };
            Reply topicReply = new Reply() { Content = "Test Content" };

            RedirectToRouteResult result = controller.Save(newTopic, topicReply) as RedirectToRouteResult;

            // Verify items were added to mockSets and correct redirect action is returned
            mockSetTopics.Verify(m => m.Add(It.IsAny<Topic>()), Times.Once());
            mockSetReplies.Verify(m => m.Add(It.IsAny<Reply>()),Times.Once());
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}

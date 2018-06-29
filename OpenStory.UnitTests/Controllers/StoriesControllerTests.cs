using NUnit.Framework;
using OpenStory.Controllers;
using OpenStory.Models;
using System.Web.Mvc;

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
    }
}

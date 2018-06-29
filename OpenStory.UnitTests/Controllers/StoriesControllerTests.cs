using NUnit.Framework;
using OpenStory.Controllers;
using OpenStory.Models;
using System.Web.Mvc;

namespace OpenStory.UnitTests.Controllers
{
     // All test method names have the following format: [methodName] _[scenario]_[expectedBehavior]
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
    }
}

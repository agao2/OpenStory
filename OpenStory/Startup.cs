using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenStory.Startup))]
namespace OpenStory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TouchTest.Web.Startup))]

namespace TouchTest.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

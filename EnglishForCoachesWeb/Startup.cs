using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnglishForCoachesWeb.Startup))]
namespace EnglishForCoachesWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

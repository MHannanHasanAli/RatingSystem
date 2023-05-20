using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RatingSystem.Startup))]
namespace RatingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
        }
    }
}

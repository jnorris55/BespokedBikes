using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BespokedBikes.Startup))]
namespace BespokedBikes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

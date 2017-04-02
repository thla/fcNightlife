using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(fcNightlife.Startup))]
namespace fcNightlife
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Century21.Startup))]
namespace Century21
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

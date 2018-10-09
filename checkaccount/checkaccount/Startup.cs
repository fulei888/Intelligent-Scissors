using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(checkaccount.Startup))]
namespace checkaccount
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

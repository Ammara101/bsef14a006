using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EadProjectShoppingApp.Startup))]
namespace EadProjectShoppingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

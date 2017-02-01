using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpenseApp.Startup))]
namespace ExpenseApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

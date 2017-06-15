using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleWebProject.Startup))]
namespace SampleWebProject

{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

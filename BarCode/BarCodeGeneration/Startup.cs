using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BarCodeGeneration.Startup))]
namespace BarCodeGeneration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

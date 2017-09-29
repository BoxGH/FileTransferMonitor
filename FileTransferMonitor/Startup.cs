using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileTransferMonitor.Startup))]
namespace FileTransferMonitor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

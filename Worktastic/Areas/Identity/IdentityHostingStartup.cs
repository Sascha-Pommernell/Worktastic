[assembly: HostingStartup(typeof(Worktastic.Areas.Identity.IdentityHostingStartup))]
namespace Worktastic.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
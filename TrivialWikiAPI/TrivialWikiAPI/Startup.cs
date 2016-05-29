using Microsoft.Owin.Cors;
using Owin;

namespace TrivialWikiAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
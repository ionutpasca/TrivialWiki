using Owin;

namespace TrivialWikiAPI.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}

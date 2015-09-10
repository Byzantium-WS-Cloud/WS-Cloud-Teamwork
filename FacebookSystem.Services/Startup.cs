using Microsoft.Owin;

[assembly: OwinStartup(typeof(FacebookSystem.Services.Startup))]

namespace FacebookSystem.Services
{
    using Microsoft.Owin.Cors;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            this.ConfigureAuth(app);
        }
    }
}

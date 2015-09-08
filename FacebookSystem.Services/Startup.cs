using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FacebookSystem.Services.Startup))]

namespace FacebookSystem.Services
{
    using System.Web.Http;

    using Newtonsoft.Json.Serialization;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}

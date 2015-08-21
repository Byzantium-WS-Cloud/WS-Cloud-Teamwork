namespace FacebookSystem.Services.Controllers
{
    using System.Web.Http;

    using FacebookSystem.Data.UnitOfWork;

    public class BaseApiController : ApiController
    {
        protected IFacebookSystemData data;

        public BaseApiController()
            :this(new FacebookSystemData())
        {
        }

        public BaseApiController(IFacebookSystemData data)
        {
            this.data = data;
        }
    }
}

namespace FacebookSystem.Services.Controllers
{
    using System.Web.Http;

    using FacebookSystem.Data.UnitOfWork;

    public class BaseApiController : ApiController
    {
        private IFacebookSystemData data;

        public BaseApiController()
            : this(new FacebookSystemData())
        {
        }

        public BaseApiController(IFacebookSystemData data)
        {
            this.Data = data;
        }

        protected IFacebookSystemData Data
        {
            get
            {
                return this.data;
            }

            private set
            {
                this.data = value;
            }
        }
    }
}

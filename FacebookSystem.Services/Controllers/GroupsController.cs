namespace FacebookSystem.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;

    public class GroupsController : BaseApiController
    {

        [HttpGet]
        public IHttpActionResult All()
        {
            var existingGroup = this.Data.Groups.All();

            return this.Ok(existingGroup);
        }
    }
}

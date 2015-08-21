namespace FacebookSystem.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;

    public class GroupsController : BaseApiController
    {
        public GroupsController()
            :base()
        {
        }

        [HttpGet]
        public IHttpActionResult All()
        {
            var existingGroup = this.data.Groups.All();

            return Ok(existingGroup);
        }
    }
}

namespace FacebookSystem.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.PostModels;

    using Microsoft.AspNet.Identity;
    using Models;

    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        // GET api/users/{username}
        [HttpGet]
        [Route("{username}")]
        public IHttpActionResult GetUser(string username)
        {
            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            if (loggedUserId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var loggedUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == loggedUserId);
            var result = UserViewModel.Create(user, loggedUser);

            return this.Ok(result);
        }
    }
}

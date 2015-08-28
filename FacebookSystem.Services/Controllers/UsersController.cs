namespace FacebookSystem.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models;

    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        // GET api/users/{username}
        [HttpGet]
        [Route("get/{username}")]
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

        // GET api/users/search/{name}
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchUserByName([FromUri] string name)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            if (loggedUserId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            name = name.ToLower();
            var userMatches = this.Data.ApplicationUsers.All()
                .Where(u => u.Name.ToLower().Contains(name))
                .Take(5)
                .AsEnumerable()
                .Select(SearchUserViewModel.Create);

            return this.Ok(userMatches);
        }

        // GET api/users/{username}/friends/preview
        [HttpGet]
        [Route("{username}/friends/preview")]
        public IHttpActionResult GetFriendsPreview(string username)
        {
            var user = this.Data.ApplicationUsers.All()
                .FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            bool isFriend = user.Friends
                .Any(fr => fr.Id == loggedUserId);

            if (!isFriend)
            {
                return this.BadRequest("Cannot access non-friend users.");
            }

            var friends = user.Friends
                .Reverse()
                .Take(6)
                .Select(MinifiedUserViewModel.Create);

            return this.Ok(new
            {
                totalCount = user.Friends.Count(),
                friends
            });
        }

        [HttpGet]
        [Route("{username}/friends")]
        public IHttpActionResult GetFriends(string username)
        {
            var user = this.Data.ApplicationUsers.All()
                .FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            bool isFriend = user.Friends
                .Any(fr => fr.Id == loggedUserId);

            if (!isFriend)
            {
                return this.BadRequest("Cannot access non-friend users.");
            }

            var friends = user.Friends
                .OrderBy(fr => fr.Name)
                .Select(MinifiedUserViewModel.Create);

            return this.Ok(friends);
        }
    }
}

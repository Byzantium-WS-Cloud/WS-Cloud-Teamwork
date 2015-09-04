namespace FacebookSystem.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
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

        // GET api/users/pesho/wall
        [HttpGet]
        [Route("{username}/wall")]
        public IHttpActionResult GetWall(string username, [FromUri]GetWallBindingModel wall)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var wallOwner = this.Data.ApplicationUsers.All()
                .FirstOrDefault(u => u.UserName == username);
            if (wallOwner == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            if (loggedUserId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var loggedUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == loggedUserId);

            var candidatePosts = wallOwner.Posts
                .OrderByDescending(p => p.CreatedOn)
                .AsQueryable();
            var t = wallOwner.Posts.OrderByDescending(p => p.CreatedOn).ToList();

            if (wall.StartPostId.HasValue)
            {
                candidatePosts = candidatePosts
                    .SkipWhile(p => p.Id != wall.StartPostId)
                    .Skip(1)
                    .AsQueryable();
            }

            var pagePosts = candidatePosts
                .Take(wall.PageSize)
                .Select(p => WallPostsViewModel.Create(p, loggedUser));

            return this.Ok(pagePosts);
        }

        // POST api/User/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<HttpResponseMessage> LoginUser(LoginUserBindingModel model)
        {
            // Invoke the "token" OWIN service to perform the login: /api/token
            // Ugly hack: I use a server-side HTTP POST because I cannot directly invoke the service (it is deeply hidden in the OAuthAuthorizationServerHandler class)
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "api/Account/Login";
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                var responseMsg = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
        }
    }
}

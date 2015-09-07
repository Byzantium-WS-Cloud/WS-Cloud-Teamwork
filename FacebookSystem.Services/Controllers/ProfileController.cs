namespace FacebookSystem.Services.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models;
    using Models.PostModels;
    using FacebookSystem.Models.Enums;
    using FacebookSystem.Models;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Data;

    [Authorize]
    [RoutePrefix("api/profile")]
    public class ProfileController : BaseApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult GetProfile()
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            
            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == userId);
            var result = ProfileViewModel.Create(user);

            return this.Ok(result);
        }

        [HttpPut]
        [Route]
        public IHttpActionResult EditProfileInfo(EditProfileBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            
            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return this.BadRequest("Invalid user token.");
            }

            var userEmail = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Email == model.Email);
            if (userEmail != null && userEmail.Id != currentUserId)
            {
                return this.BadRequest("Email is already taken.");
            }

            currentUser.Name = model.Name;
            currentUser.Email = model.Email;
            currentUser.Gender = model.Gender;

            if (model.ProfileImageData != null && model.ProfileImageData.IndexOf(',') == -1)
            {
                model.ProfileImageData = string.Format("{0}{1}", "data:image/jpg;base64,", model.ProfileImageData);
            }

            currentUser.ProfileImageData = model.ProfileImageData;

            if (model.CoverImageData != null && model.CoverImageData.IndexOf(',') == -1)
            {
                model.CoverImageData = string.Format("{0}{1}", "data:image/jpg;base64,", model.CoverImageData);
            }

            currentUser.CoverImageData = model.CoverImageData;

            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "User profile updated successfully."
            });
        }

        [HttpGet]
        [Route("friends")]
        public IHttpActionResult GetFriends()
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest();
            }

            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == userId);
            var friends = user.Friends
                .OrderBy(fr => fr.Name)
                .Select(UserViewModelMinified.Create);

            return this.Ok(friends);
        }

        [HttpGet]
        [Route("feed")]
        public IHttpActionResult GetNewsFeed([FromUri]NewsFeedBindingModel newsFeedModel)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest();
            }

            if (newsFeedModel == null)
            {
                return this.BadRequest("No page size specified.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == userId);
            
            var targetFeedPosts = user.WallPosts
                .OrderByDescending(p => p.CreatedOn)
                .AsQueryable();

            if (newsFeedModel.StartPostId.HasValue)
            {
                targetFeedPosts =
                    targetFeedPosts.SkipWhile(p => p.Id != newsFeedModel.StartPostId)
                    .Skip(1).AsQueryable();
            }

            var pagePosts = targetFeedPosts
                .Take(newsFeedModel.PageSize)
                .Select(p => WallPostsViewModel.Create (p, user));


            return this.Ok(pagePosts);
        }
        
        [HttpGet]
        [Route("requests")]
        public IHttpActionResult GetFriendRequests()
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest();
            }

            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == userId);
            var friendRequests = user.FriendRequests
                .Where(r => r.Status == FriendRequestStatus.Pending)
                .Select(FriendRequestViewModel.Create);

            return this.Ok(friendRequests);
        }

        [HttpPost]
        [Route("requests/{username}")]
        public IHttpActionResult SendFriendRequest(string username)
        {
            var recipient = this.Data.ApplicationUsers.All()
                .FirstOrDefault(u => u.UserName == username);
            if (recipient == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            if (loggedUserId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var loggedUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == loggedUserId);
            if (username == loggedUser.UserName)
            {
                return this.BadRequest("Cannot send request to self.");
            }

            bool isAlreadyFriend = loggedUser.Friends
                .Any(fr => fr.UserName == recipient.UserName);
            if (isAlreadyFriend)
            {
                return this.BadRequest("User is already in friends.");
            }

            bool hasReceivedRequest = loggedUser.FriendRequests
                .Any(r => r.FromId == recipient.Id && r.Status == FriendRequestStatus.Pending);
            bool hasSentRequest = recipient.FriendRequests
                .Any(r => r.FromId == loggedUser.Id && r.Status == FriendRequestStatus.Pending);
            if (hasSentRequest || hasReceivedRequest)
            {
                return this.BadRequest("A pending request already exists.");
            }

            var friendRequest = new FriendRequest()
            {
                From = loggedUser,
                To = recipient
            };

            recipient.FriendRequests.Add(friendRequest);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = string.Format("Friend request successfully sent to {0}.", recipient.Name)
            });
        }

        [HttpPut]
        [Route("requests/{requestId}")]
        public IHttpActionResult ChangeRequestStatus(int requestId, [FromUri] string status)
        {
            var request = this.Data.FriendRequests.All().FirstOrDefault(fr => fr.Id == requestId);
            if (request == null)
            {
                return this.NotFound();
            }

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (request.Status != FriendRequestStatus.Pending)
            {
                return this.BadRequest("Request status is already resolved.");
            }

            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == userId);
            if (request.ToId != userId)
            {
                return this.BadRequest("Friend request belongs to different user.");
            }

            if (status == "approved")
            {
                request.Status = FriendRequestStatus.Approved;
                user.Friends.Add(request.From);
                request.From.Friends.Add(user);
            }
            else if (status == "rejected")
            {
                request.Status = FriendRequestStatus.Rejected;
            }
            else
            {
                return this.BadRequest("Invalid friend request status.");
            }

            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = string.Format("Friend request successfully {0}.", status)
            });
        }
    }
}

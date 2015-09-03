namespace FacebookSystem.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.PostModels;

    using Microsoft.AspNet.Identity;

    [Authorize]
    [RoutePrefix("api/posts")]
    public class PostsController : BaseApiController
    {
	// Comment for Georgi
        // GET api/posts/GetAllPosts
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllPosts")]
        public IHttpActionResult GetAllPosts()
        {
            var posts = this.Data.Posts.All().Where(p => p.IsPostHidden == false).Select(AllPostsViewModel.Create);

            return this.Ok(posts);
        }

        // GET api/posts/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetPostById(int id)
        {
            var post = this.Data.Posts.All().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            return
                this.Ok(new
                        {
                            post.Id,
                            post.Content,
                            post.CreatedOn,
                            Owner = post.Owner.UserName,
                            Comments = post.Comments.Select(c => c.Content),
                            Likes = post.Likes.Count()
                        });
        }

        // POST api/posts/CreatePost
        [HttpPost]
        [Route("CreatePost")]
        public IHttpActionResult CreatePost(CreatePostBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var post = new Post { Content = model.Content, OwnerId = currentUserId, CreatedOn = DateTime.Now };

            this.Data.Posts.Add(post);
            this.Data.SaveChanges();
            return this.Ok(new { post.Id, post.Content });
        }

        // DELETE api/posts/{id}/Delete
        [HttpDelete]
        [Route("{id}/Delete")]
        [ActionName("Delete")]
        public IHttpActionResult DeletePostById(int id)
        {
            var post = this.Data.Posts.All().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();
            if (post.OwnerId != currentUserId)
            {
                return this.BadRequest("You are not owner of this post. You cannot delete it!");
            }

            if (post.IsPostHidden == true)
            {
                return this.BadRequest("Post does not exists");
            }

            post.IsPostHidden = true;
            this.Data.SaveChanges();
            return this.Ok("Post has been successfully deleted!");
        }

        // POST api/{id}/like
        [HttpPost]
        [Route("{id}/like")]
        public IHttpActionResult LikePost(int id)
        {
            var post = this.Data.Posts.All().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();

            var postAlreadyLiked = this.Data.PostLikes.All().Any(l => l.PostId == post.Id && l.UserId == currentUserId);
            if (postAlreadyLiked)
            {
                return this.BadRequest("You have already liked this post.");
            }

            var postLike = new PostLike() 
            {
                PostId = id,
                UserId = currentUserId
            };

            post.Likes.Add(postLike);
            this.Data.SaveChanges();
            
            return this.Ok("Successfully  liked post " + post.Id);
        }

        // PUT api/{id}/dislike
        [HttpDelete]
        [Route("{id}/dislike")]
        public IHttpActionResult DislikePost(int id)
        {
            var post = this.Data.Posts.All().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();

            var postAlreadyLiked = this.Data.PostLikes.All().Any(l => l.PostId == post.Id && l.UserId == currentUserId);
            if (!postAlreadyLiked)
            {
                return this.BadRequest("Cannot dislike not liked post.");
            }

            var postLike = this.Data.PostLikes.All().FirstOrDefault(l => l.PostId == post.Id);
            
            post.Likes.Remove(postLike);
            postLike.PostId = null;
            postLike.UserId = null;
            this.Data.SaveChanges();

            return this.Ok("Successfully  disliked post " + post.Id);
        }
    }
}

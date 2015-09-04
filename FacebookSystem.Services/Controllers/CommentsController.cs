namespace FacebookSystem.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.CommentModels;

    using Microsoft.AspNet.Identity;

    [Authorize]
    public class CommentsController : BaseApiController
    {
        //Get api/posts/{id}/comments
        [HttpGet]
        [Route("api/posts/{postId}/comments")]
        public IHttpActionResult GetPostComments(int postId)
        {
            var post = this.Data.Posts.All().FirstOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return this.BadRequest(String.Format("there is no post with id {0}", postId));
            }

            if (post.IsPostHidden)
            {
                return this.Unauthorized();
            }

            var comments = this.Data.Comments.All()
                .Where(c => c.PostId == postId && c.IsCommentHidden == false)
                .OrderBy(c => c.CreatedOn)
                .Select(CommentViewModel.Create);

            return this.Ok(comments);
        }

        //Get api/comments/{id}
        [HttpGet]
        [Route ("api/comments/{id}")]
        public IHttpActionResult GetCommentById(int id)
        {
            var comment = this.Data.Comments.All().FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return this.NotFound();
            }

            var result = CommentViewModel.Create1(comment);
            return this.Ok(result);
        }


        //Post api/posts/{id}/comments
        [HttpPost]
        [Route("api/posts/{postId}/comments")]
        public IHttpActionResult AddComment(int postId, CommentBindingModel model)
        {
            var post = this.Data.Posts.All().FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return this.BadRequest(String.Format("there is no post with id {0}", postId));
            }

            if (post.IsPostHidden)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);

            var comment = new Comment()
                              {
                                  Content = model.Content,
                                  PostId = postId,
                                  CreatedOn = DateTime.Now,
                                  CommentOwner = loggedUser,
                                  PostOwner = post.Owner
                              };

            this.Data.Comments.Add(comment);
            this.Data.SaveChanges();

            var postedComment =
                this.Data.Comments.All()
                .Where(c => c.Id == comment.Id).
                Select(CommentViewModel.Create)
                .FirstOrDefault();

            return this.Ok(postedComment);
        }

        //Put /comments/{id}
        [HttpPut]
        [Route ("api/comments/{commentId}")]
        public IHttpActionResult EditComment(int commentId, CommentBindingModel model)
        {
            
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            
            var comment = this.Data.Comments.All().FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();

            // or currentUserId != comment.PostWallOwnerId
            if (currentUserId != comment.CommentOwner.Id)
            {
                return this.Unauthorized();
            }

            comment.Content = model.Content;
            this.Data.SaveChanges();

            var viewModel = this.Data.Comments.All().Where(c => c.Id == commentId).Select(CommentViewModel.Create);
            return this.Ok(viewModel);

        }

        //Get /comments/{id}/likes
        [HttpGet]
        [Route ("api/comments/{id}/likes")]
        public IHttpActionResult GetCommentLikes(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            int likesCount =
                this.Data.CommentLikes.All().Count(l => l.CommentId == id && l.UserId == currentUserId);

            return this.Ok(likesCount);
        }

        [HttpDelete]
        [Route ("api/comments/{id}")]
        public IHttpActionResult DeleteComment(int id)
        {
            var comment = this.Data.Comments.All().FirstOrDefault(c => c.Id == id);
            if (comment == null || comment.IsCommentHidden)
            {
                return this.BadRequest(String.Format("there is no comment with id {0}", id));
            }

            var currentUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);

            if (comment.CommentOwner != loggedUser)
            {
                return this.Unauthorized();
            }
            // check if the comment is on users wall

            comment.IsCommentHidden = true;
            this.Data.SaveChanges();

            return this.Ok();
        }

        // Make likes controler

        //Post api/comments/{id}/likes
        [HttpPost]
        [Route("api/comments/{commentId}/likes")]
        public IHttpActionResult LikeComment(int commentId)
        {
            var comment = this.Data.Comments.All().FirstOrDefault(c => c.Id == commentId);
            if (comment == null || comment.IsCommentHidden)
            {
                return this.BadRequest(String.Format("there is no comment with id {0}", commentId));
            }

            var currentUserId = this.User.Identity.GetUserId();
            var isAlreadyLiked =
                this.Data.CommentLikes.All()
                    .Any(l => l.User.Id == currentUserId && l.Comment.Id == commentId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You already liked comment with id " + commentId);
            }

            var like = new CommentLike() { CommentId = commentId, UserId = currentUserId };
            this.Data.CommentLikes.Add(like);
            this.Data.SaveChanges();

            int likesCount =
                this.Data.CommentLikes.All().Count(l => l.User.Id == currentUserId && l.Comment.Id == commentId);

            return this.Ok(likesCount);
        }

        //Delete /comments/{id}/likes
        [HttpDelete]
        [Route("api/comments/{commentId}/likes")]
        public IHttpActionResult DislikeComment(int commentId)
        {
            var comment = this.Data.Comments.All().FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();
            var like =
                this.Data.CommentLikes.All()
                    .FirstOrDefault(l => l.User.Id == currentUserId && l.Comment.Id == commentId);

            if (like == null)
            {
                return this.BadRequest(String.Format("you didn't like comment with id {0}", commentId));
            }

            comment.Likes.Remove(like);
            like.CommentId = null;
            like.UserId = null;
            this.Data.SaveChanges();

            int likesCount =
                this.Data.CommentLikes.All().Count(l => l.User.Id == currentUserId && l.Comment.Id == commentId);

            return this.Ok(likesCount);
        }
    }
}

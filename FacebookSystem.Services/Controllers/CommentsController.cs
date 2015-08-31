namespace FacebookSystem.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [Authorize]
    [RoutePrefix("api/posts/{id}/comments")]
    public class CommentsController : BaseApiController
    {
        //Get api/posts/{id}/comments
        public IHttpActionResult GetPostComments(int postId)
        {
            throw new NotImplementedException();
        }

        //Post api/posts/{id}/comments
        public IHttpActionResult AddComment(int postId)
        {
            throw new NotImplementedException();
        }

        //Put /comments/{id}
        public IHttpActionResult EditComment(int postId, int commentId)
        {
            throw new NotImplementedException();
        }

        //Get comments/{id}/likes
        public IHttpActionResult GetCommentLikes(int postId, int commentId)
        {
            throw new NotImplementedException();
        }

        //Delete /comments/{id}

        //Post api/posts/{id}/comments/{id}/likes
        public IHttpActionResult LikeComment(int postId, int commentId)
        {
            throw new NotImplementedException();            
        }

        //Delete {id}/likes
        public IHttpActionResult DislikeComment(int postId, int commentId)
        {
            throw new NotImplementedException();            
        }

    }
}

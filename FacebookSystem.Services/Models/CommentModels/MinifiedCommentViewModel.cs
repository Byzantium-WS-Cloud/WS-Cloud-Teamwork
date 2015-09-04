namespace FacebookSystem.Services.Models.CommentModels
{
    using System;
    using System.Linq.Expressions;

    using FacebookSystem.Models;


    public class MinifiedCommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public OwnerViewModel CommentOwner { get; set; }

        public int LikesCount { get; set; }

        public static Expression<Func<Comment, MinifiedCommentViewModel>> Create
        {
            get
            {
                return
                    comment =>
                        new MinifiedCommentViewModel()
                       {
                           Id = comment.Id,
                           Content = comment.Content,
                           CreatedOn = comment.CreatedOn,
                           CommentOwner =
                               new OwnerViewModel()
                                   {
                                       Id = comment.CommentOwner.Id,
                                       Username = comment.CommentOwner.UserName
                                   },
                           LikesCount = comment.Likes.Count
                       };
            }
        }
    }
}
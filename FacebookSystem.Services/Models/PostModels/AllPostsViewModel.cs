namespace FacebookSystem.Services.Models.PostModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.CommentModels;


    public class AllPostsViewModel
    {
        public static Expression<Func<Post, AllPostsViewModel>> Create
        {
            get
            {
                return
                    a =>
                    new AllPostsViewModel()
                    {
                        Id = a.Id,
                        Content = a.Content,
                        CreatedOn = a.CreatedOn,
                        Owner =
                            new OwnerViewModel() { Id = a.Owner.Id, Username = a.Owner.UserName },
                        Likes = a.Likes.Count(),
                        Comments =
                            a.Comments.Select(c => new MinifiedCommentViewModel()
                       {
                           Id = c.Id,
                           Content = c.Content,
                           CreatedOn = c.CreatedOn,
                           CommentOwner =
                               new OwnerViewModel()
                                   {
                                       Id = c.CommentOwner.Id,
                                       Username = c.CommentOwner.UserName
                                   },
                           LikesCount = c.Likes.Count
                       })
                    };
            }
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public OwnerViewModel Owner { get; set; }

        public int Likes { get; set; }

        public virtual IEnumerable<MinifiedCommentViewModel> Comments { get; set; }
    }
}
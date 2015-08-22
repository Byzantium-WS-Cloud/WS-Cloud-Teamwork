namespace FacebookSystem.Services.Models.PostModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.CommentModels;
    using FacebookSystem.Services.Models.LikeModels;

    using Microsoft.Ajax.Utilities;

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
                        Likes = a.Likes.Select(l => new LikeViewModel()
                                                    {
                                                        Likes = l.Post.Likes.Count()
                                                    }),
                        Comments =
                            a.Comments.Select(
                                c => new CommentViewModel() { Id = c.Id, Content = c.Content })
                    };
            }
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public OwnerViewModel Owner { get; set; }

        public IEnumerable<LikeViewModel> Likes { get; set; }

        public virtual IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
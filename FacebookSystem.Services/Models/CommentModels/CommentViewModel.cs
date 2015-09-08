namespace FacebookSystem.Services.Models.CommentModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.LikeModels;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserViewModelMinified Author { get; set; }

        public OwnerViewModel PostOwner { get; set; }

        public OwnerViewModel CommentOwner { get; set; }

        public int LikesCount { get; set; }

        public bool IsLiked { get; set; }

        public LikeViewModel Likes { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> Create
        {
            get
            {
                return
                    c =>
                    new CommentViewModel()
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
                            PostOwner =
                                new OwnerViewModel()
                                    {
                                        Id = c.PostOwner.Id,
                                        Username = c.PostOwner.UserName
                                    },
                            Likes = new LikeViewModel() { Likes = c.Likes.Count }
                        };
            }
        }

        public static CommentViewModel Create1(Comment c)
        {
            return new CommentViewModel()
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
                           PostOwner =
                               new OwnerViewModel()
                                   {
                                       Id = c.PostOwner.Id,
                                       Username = c.PostOwner.UserName
                                   },
                           Likes = new LikeViewModel() { Likes = c.Likes.Count }
                       };
        }

        public static CommentViewModel CreateCommentAndUser(Comment c, ApplicationUser currentUser)
        {
            return new CommentViewModel()
                       {
                           Id = c.Id,
                           Author =
                               new UserViewModelMinified()
                                   {
                                       Id = c.CommentOwner.Id,
                                       Gender = c.CommentOwner.Gender,
                                       Name = c.CommentOwner.Name,
                                       ProfileImageData =
                                           c.CommentOwner.ProfileImageData,
                                       Username = c.CommentOwner.UserName
                                   },
                           LikesCount = c.Likes.Count,
                           Content = c.Content,
                           CreatedOn = c.CreatedOn,
                           IsLiked = c.Likes.Any(l => l.UserId == currentUser.Id)
                       };
        }
    }
}
namespace FacebookSystem.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.CommentModels;
    using FacebookSystem.Services.Models.GroupModels;

    public class WallPostsViewModel
    {
        public int Id { get; set; }

        public UserViewModelMinified Author { get; set; }

        public OwnerViewModel WallOwner { get; set; }

        public string PostContent { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static WallPostsViewModel Create(Post p, ApplicationUser currentUser)
        {
            return new WallPostsViewModel()
            {
                Id = p.Id,
                Author = new UserViewModelMinified()
                             {
                                 Id = p.OwnerId,
                                 Gender = p.Owner.Gender,
                                 Name = p.Owner.Name,
                                 ProfileImageData = p.Owner.ProfileImageData,
                                 Username = p.Owner.UserName
                             },
                WallOwner = new OwnerViewModel {Id = p.OwnerId, Username = p.Owner.UserName},
                PostContent = p.Content,
                Date = p.CreatedOn,
                LikesCount = p.Likes.Count,
                Liked = p.Likes
                    .Any(l => l.UserId == currentUser.Id),
                TotalCommentsCount = p.Comments.Count,
                Comments = p.Comments
                    .Take(3)
                    .Select(c => CommentViewModel.CreateCommentAndUser(c, currentUser))
            };
        }
    }
}
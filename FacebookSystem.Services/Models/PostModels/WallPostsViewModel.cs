namespace FacebookSystem.Services.Models.PostModels
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

        public UserViewModel Author { get; set; }

        public UserViewModel WallOwner { get; set; }

        public string PostContent { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static PostViewModel Create(Post p, ApplicationUser currentUser)
        {
            return new PostViewModel()
            {
                Id = p.Id,
              //  Owner = UserViewModel.Create(p.Owner, currentUser),
              //  WallOwner = UserViewModel.Create(p.WallOwner, currentUser),
                Content = p.Content,
                CreatedOn = p.CreatedOn,
                Likes = p.Likes.Count,
                TotalCommentsCount = p.Comments.Count,
                Comments = p.Comments
                    .Reverse()
                    .Take(3)
                    .Select(c => CommentViewModel.Create(c))
            };
        } 
    }
}
namespace FacebookSystem.Services.Models
{
    using System.Linq;

    using FacebookSystem.Models;
    using FacebookSystem.Models.Enums;

    public class UserViewModelPreview
    {
        public string Id { get; set; }

        public string ProfileImageData { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public bool IsFriend { get; set; }

        public bool HasPendingRequest { get; set; }

        public static UserViewModelPreview Create(ApplicationUser user, ApplicationUser loggedUser)
        {
            return new UserViewModelPreview()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Gender = user.Gender,
                ProfileImageData = user.ProfileImageDataMinified,
                IsFriend = user.Friends
                   .Any(fr => fr.Id == loggedUser.Id),
                HasPendingRequest = user.FriendRequests
                    .Any(r => r.Status == FriendRequestStatus.Pending &&
                        (r.FromId == loggedUser.Id || r.ToId == loggedUser.Id))
            };
        } 
    }
}
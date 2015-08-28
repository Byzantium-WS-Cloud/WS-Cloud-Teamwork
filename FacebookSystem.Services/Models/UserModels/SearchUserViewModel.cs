using FacebookSystem.Models;
using FacebookSystem.Models.Enums;

namespace FacebookSystem.Services.Models
{
    public class SearchUserViewModel
    {
        public string Id { get; set; }

        public string ProfileImageData { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public static SearchUserViewModel Create(ApplicationUser user)
        {
            return new SearchUserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Gender = user.Gender,
                ProfileImageData = user.ProfileImageDataMinified
            };
        }
    }
}
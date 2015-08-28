using FacebookSystem.Models;
using FacebookSystem.Models.Enums;
using System.Linq;

namespace FacebookSystem.Services.Models
{
    public class MinifiedUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string ProfileImageData { get; set; }

        public Gender Gender { get; set; }
        
        public static MinifiedUserViewModel Create(ApplicationUser user)
        {
            return new MinifiedUserViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                ProfileImageData = user.ProfileImageData,
                Gender = user.Gender
            };
        }
    }
}
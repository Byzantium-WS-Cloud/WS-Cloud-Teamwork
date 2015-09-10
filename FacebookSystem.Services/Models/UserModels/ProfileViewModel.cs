using FacebookSystem.Models;
using FacebookSystem.Models.Enums;
using System.Linq;

namespace FacebookSystem.Services.Models
{
    using System;
    using System.Linq.Expressions;

    public class ProfileViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ProfileImageData { get; set; }

        public Gender Gender { get; set; }

        public string CoverImageData { get; set; }

        public static Expression<Func<ApplicationUser, ProfileViewModel>> Create
        {
            get
            {
                return
                    user =>
                    new ProfileViewModel()
                        {
                            Id = user.Id,
                            Username = user.UserName,
                            Name = user.Name,
                            Email = user.Email,
                            ProfileImageData = user.ProfileImageData,
                            Gender = user.Gender,
                            CoverImageData = user.CoverImageData
                        };
            }
        }
    }
}
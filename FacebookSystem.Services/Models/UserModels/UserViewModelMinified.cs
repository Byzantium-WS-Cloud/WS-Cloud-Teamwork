namespace FacebookSystem.Services.Models
{
    using System;
    using System.Linq.Expressions;

    using FacebookSystem.Models;
    using FacebookSystem.Models.Enums;

    public class UserViewModelMinified
    {
        public string Id { get; set; }

        public string ProfileImageData { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public static Expression<Func<ApplicationUser, UserViewModelMinified>> Create
        {
            get
            {
                return
                    user =>
                    new UserViewModelMinified()
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
}
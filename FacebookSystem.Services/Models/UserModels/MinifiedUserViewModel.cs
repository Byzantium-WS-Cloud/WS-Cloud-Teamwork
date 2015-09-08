using FacebookSystem.Models;
using FacebookSystem.Models.Enums;
using System.Linq;

namespace FacebookSystem.Services.Models
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;

    public class MinifiedUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string ProfileImageData { get; set; }

        public Gender Gender { get; set; }

        public static Expression<Func<ApplicationUser, MinifiedUserViewModel>> Create
        {
            get
            {
                return
                    user =>
                    new MinifiedUserViewModel()
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
}
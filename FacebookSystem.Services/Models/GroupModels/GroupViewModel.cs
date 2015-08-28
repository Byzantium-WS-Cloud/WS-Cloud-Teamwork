using System;
using System.Collections.Generic;
using FacebookSystem.Services.Controllers;

namespace FacebookSystem.Services.Models.GroupModels
{
    public class GroupViewModel
    {
        public GroupViewModel()
        {
            this.Members = new List<GroupUserViewModel>();
            this.Posts = new List<PostViewModel>();
        }

        public string Name { get; set; }

        public DateTime CreateOn { get; set; }

        public ICollection<GroupUserViewModel> Members { get; set; }

        public ICollection<PostViewModel> Posts { get; set; }
    }
}
using FacebookSystem.Services.Models;
using FacebookSystem.Services.Models.GroupModels;

namespace FacebookSystem.Services.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FacebookSystem.Models;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Linq;
    using Models.PostModels;
    using Microsoft.AspNet.Identity;
    using WebGrease.Css.Extensions;

    [Authorize]
    public class GroupsController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult CreateGroup(CreateGroupBindingModel group)
        {
            if (group == null)
            {
                return this.BadRequest("Group model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (this.Data.Groups.All().Any(g => g.Name == group.Name))
            {
                return this.BadRequest("The group name already exists");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);
            this.Data.Groups.Add(new Group()
            {
                Name = group.Name,
                CreatedOn = DateTime.Now,
                Members = new List<ApplicationUser>()
                {
                    currentUser
                }
            });
            this.Data.SaveChanges();

            var viewModel = new CreateGroupViewModel()
            {
                Name = group.Name
            };

            return this.Ok(viewModel);
        }

        [HttpGet]
        public IHttpActionResult FindGroupById(int id)
        {
            var group = this.Data.Groups.All().SingleOrDefault(g => g.Id == id);

            if (group == null)
            {
                return this.BadRequest("Invalid group id");
            }

            var postViewModel = new List<PostViewModel>();
            group.Posts.ToList().ForEach(p =>
            {
                postViewModel.Add(new PostViewModel()
                {
                    Id = p.Id,
                    Likes = p.Likes.Count,
                    Content = p.Content,
                    Owner = new GroupUserViewModel()
                    {
                        UserName = p.Owner.UserName,
                        Id = p.OwnerId
                    }
                });
            });

            var userViewModel = new List<GroupUserViewModel>();
            group.Members.ForEach(m =>
            {
                userViewModel.Add(new GroupUserViewModel()
                {
                    UserName = m.UserName,
                    Id = m.Id
                });
            });

            var groupModel = new GroupViewModel()
            {
                Name = group.Name,
                CreateOn = group.CreatedOn,
                Posts = postViewModel,
                Members = userViewModel
            };

            return this.Ok(groupModel);
        }

        public IHttpActionResult AddPost(int groupId, CreatePostBindingModel post)
        {
            var group = this.Data.Groups.All().SingleOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return this.BadRequest("Invalid group id");
            }

            if (post == null)
            {
                return this.BadRequest("Post model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var postOwnerId = this.User.Identity.GetUserId();
            var postOwner = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == postOwnerId);

            if (!group.Members.Any(m => m.Id == postOwnerId))
            {
                return this.BadRequest("You are not member of the group");
            }

            var postObj = new Post()
            {
                Content = post.Content,
                CreatedOn = DateTime.Now,
                Owner = postOwner,
                OwnerId = postOwnerId
            };

            group.Posts.Add(postObj);
            this.Data.SaveChanges();

            var viewModel = new PostViewModel()
            {
                Owner = new GroupUserViewModel()
                {
                    Id = postObj.OwnerId,
                    UserName = postObj.Owner.UserName
                },
                Content = postObj.Content
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public IHttpActionResult Join(int groupId)
        {
            var group = this.Data.Groups.All().SingleOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return this.BadRequest("Invalid group id");
            }

            var currentUserId = this.User.Identity.GetUserId();

            if (group.Members.Any(m => m.Id == currentUserId))
            {
                return this.BadRequest("You are already member of this group");
            }   

            var currentUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);
            group.Members.Add(currentUser);
            this.Data.SaveChanges();

            return this.Ok(string.Format("You successfuly join the {0} group", group.Name));
        }
        
        [HttpPut]
        public IHttpActionResult Leave(int groupId)
        {
            var group = this.Data.Groups.All().FirstOrDefault(g => g.Id == groupId);
            var currentUserId = this.User.Identity.GetUserId();

            if (group == null)
            {
                return this.BadRequest("Invalid group id");
            }

            var member = group.Members.SingleOrDefault(m => m.Id == currentUserId);
            if (member == null)
            {
                return this.BadRequest("You are not member of this group");
            }

            group.Members.Remove(member);
            this.Data.SaveChanges();

            return this.Ok(string.Format("You successfuly leave {0} group", group.Name));
        }

        // You can like post even if you are not member
        public IHttpActionResult LikePost(int postId, int groupId)
        {
            var group = this.Data.Groups.All().SingleOrDefault(g => g.Id == groupId);

            if (group == null)
            {
                return this.BadRequest("Invalid group id");
            }

            var post = group.Posts.SingleOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return this.BadRequest("Invalid post id");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);

            if (post.Likes.Any(l => l.UserId == currentUserId))
            {
                return this.BadRequest("You have already liked this post");
            }

            post.Likes.Add(new PostLike()
            {
                Post = post,
                PostId = post.Id,
                User = currentUser,
                UserId = currentUser.Id
            });
            this.Data.SaveChanges();

            return this.Ok("You successfully like this post");
        }

        public IHttpActionResult DeletePost()
        {
            throw new NotImplementedException();
        }



        [HttpGet]
        public IHttpActionResult All()
        {
            var existingGroup = this.Data.Groups.All();

            return this.Ok(existingGroup);
        }
    }
}

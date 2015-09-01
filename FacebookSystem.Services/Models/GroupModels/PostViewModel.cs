namespace FacebookSystem.Services.Models.GroupModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public GroupUserViewModel Owner { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }
    }
}
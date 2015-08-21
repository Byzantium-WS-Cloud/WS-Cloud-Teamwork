namespace FacebookSystem.Models
{
    public class PostLike
    {
        public int Id { get; set; }

        public Post Post { get; set; }

        public ApplicationUser User { get; set; }
    }
}

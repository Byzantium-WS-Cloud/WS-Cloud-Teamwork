namespace FacebookSystem.Models
{
    public class CommentLike
    {
        public int Id { get; set; }

        public Comment Comment { get; set; }

        public ApplicationUser User { get; set; }
    }
}

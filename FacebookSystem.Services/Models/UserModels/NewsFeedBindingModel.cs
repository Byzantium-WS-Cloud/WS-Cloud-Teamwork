namespace FacebookSystem.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    public class NewsFeedBindingModel
    {
        public int? StartPostId { get; set; }

        [Required]
        [Range(0, 10)]
        public int PageSize { get; set; }
    }
}
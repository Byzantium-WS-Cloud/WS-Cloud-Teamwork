namespace FacebookSystem.Services.Models.CommentModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCommentBindingModel
    {
        [Required]
        [MinLength(2)]
        public string Content { get; set; }
    }
}
namespace FacebookSystem.Services.Models.CommentModels
{
    using System.ComponentModel.DataAnnotations;

    public class CommentBindingModel
    {
        [Required]
        [MinLength(2)]
        public string Content { get; set; }
    }
}
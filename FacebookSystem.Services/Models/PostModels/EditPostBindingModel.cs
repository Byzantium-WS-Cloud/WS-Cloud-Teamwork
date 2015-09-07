namespace FacebookSystem.Services.Models.PostModels
{
    using System.ComponentModel.DataAnnotations;

    public class EditPostBindingModel
    {
        [Required]
        public string Content { get; set; } 
    }
}
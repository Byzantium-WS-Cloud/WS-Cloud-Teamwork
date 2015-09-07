namespace FacebookSystem.Services.Models.PostModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreatePostBindingModel
    {
        [Required]
        [MinLength(2)]
        public string Content { get; set; }

        [Required]
        public string WallOwnerUsername { get; set; }
    }
}
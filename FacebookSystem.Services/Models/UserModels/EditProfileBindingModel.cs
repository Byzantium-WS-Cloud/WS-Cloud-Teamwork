namespace FacebookSystem.Services.Models
{
    using FacebookSystem.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class EditProfileBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Profile image")]
        public string ProfileImageData { get; set; }

        [Display(Name = "Cover image")]
        public string CoverImageData { get; set; }
    }
}
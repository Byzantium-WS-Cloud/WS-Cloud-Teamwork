namespace FacebookSystem.Services.Models.GroupModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateGroupViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
namespace FacebookSystem.Services.Models.GroupModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateGroupBindingModel
    {
        [Required]
        public string Name { get; set; }
    }
}
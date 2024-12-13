using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Data.Entities;

namespace FinalProject.Web.Models.User;
public class ProfileViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Remote(action: "VerifyEmailAvailable", controller: "User", AdditionalFields = nameof(Id))]
    public string Email { get; set; }

    public Role Role { get; set; }
    [Required]
    [Display(Name = "Dog Breed")]
    public string DogBreed { get; set; }
    [Display(Name = "Profile Image")]
    public IFormFile ProfileImage { get; set; }
    public string ProfileImageUrl { get; set; }

}
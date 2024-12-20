﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Web.Models.User;

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Remote(action: "VerifyEmailAvailable", controller: "User")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
    public string PasswordConfirm { get; set; }
    [Display(Name = "Dog Breed")]
    public string DogBreed { get; set; }
    [Display(Name = "Profile Image")]
    public IFormFile ProfileImage { get; set; }

}
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Entities;

public class Post 
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }

    //user owning the post
    public int UserId { get; set; }
    public User User { get; set; }
    }

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data.Entities;

public class Post 
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    public string Content { get; set; }
    public DateTime ModifiedAt { get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string ImagePath { get; set; }

    //Navigate property to include comments
    public IEnumerable<Comment> Comments { get; set; }

    //user owning the post
    public int UserId { get; set; }
    public User User { get; set; }
    }

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data.Entities;

public class Post 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
   // public DateTime ModifiedAt { get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string ImagePath { get; set; }
    public DateTime PostedOn { get; set; } = DateTime.Now;

    public IList<Comment> Comments { get; set; } = new List<Comment>();
    public IList<Post> Posts { get; set; } = [];
    public string CreatedBy { get; set; }

    //Navigate property to include comments
    //public IEnumerable<Comment> Comments { get; set; }

    //user owning the post
    public int? UserId { get; set; }
    public User User { get; set; }
    }

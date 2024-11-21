using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FinalProject.Data.Entities;

public class Comment
{
    public int Id { get; set; }
    [Required]
    //public string Content { get; set; }
    public string Comments { get; set; }
    public int PostId { get; set; }
    public Post Post{ get; set; }
    public int? UserId { get; set; }
    public User User{ get; set; }
    public string ImagePath { get; set; }
    public string Name { get; set; }
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    //public DateTime ModifiedAt { get; set;}



}
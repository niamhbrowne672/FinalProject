using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Web.Models;

public class CommentModel
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set;}
    public string UserName { get; set;}
}
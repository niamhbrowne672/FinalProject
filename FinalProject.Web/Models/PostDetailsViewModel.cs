using System;
namespace FinalProject.Web.Models;
public class PostDetailsViewModel
{
    public int Id { get; set;}
    public string Title { get; set;}
    public string Content { get; set;}
    public List<CommentModel> Comments { get; set;} = new List<CommentModel>();
}

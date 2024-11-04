using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinalProject.Web.Models;

using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Security;
using FinalProject.Web.Models.User;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.Configuration.UserSecrets;

/**
* Post Controller
*/
namespace FinalProject.Web.Controllers;

public class PostController : BaseController
{
    private readonly IPostService _svc;

    public PostController(IPostService svc)
    {
        _svc = svc;
    }

    //HTTP GET - Display Paged List of Posts

    public IActionResult Index(int page=1, int size=20, string order="id", string direction="asc")
    {
        var paged = _svc.GetPosts(page,size,order,direction);
        return View(paged);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    //HTTP POST - Create a new Post
    [Authorize]
    [HttpPost]
    public IActionResult Create(Post post)
    {
        if (ModelState.IsValid)
        {
            post.UserId = User.GetSignedInUserId();
            _svc.AddPost(post);
            return RedirectToAction("Index");
        }
        return View(post);
    }

    public IActionResult Details(int id)
    {
        var post = _svc.GetPostById(id);
        if (post == null) return NotFound();

        var viewModel = new PostDetailsViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Comments = post.Comments.Select(c => new CommentModel { 
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserName = c.User.Name
                }).ToList()
        };
        return View(viewModel);
    }


    //Comment section

    [HttpPost]
    [Authorize]
    public IActionResult AddComment(int postId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ModelState.AddModelError("", "Comment cannot be empty.");
            return RedirectToAction("Details", new { id = postId });
        }
        var comment = new Comment
        {
            Content = content,
            PostId = postId,
            UserId = User.GetSignedInUserId(),
            CreatedAt = DateTime.Now
        };

        _svc.AddComment(postId, comment);
        return RedirectToAction("Details", new { id = postId });
    }

    [HttpGet]
    [Authorize]
    public IActionResult EditComment(int id)
    {
        var comment = _svc.GetCommentById(id);
        if (comment == null || comment.UserId != User.GetSignedInUserId()) return Forbid();
        return View(comment);
    }

    [HttpPost]
    [Authorize]
    public IActionResult EditComment(Comment comment)
    {
        if (!ModelState.IsValid) return View(comment);
        var existingComment = _svc.GetCommentById(comment.Id);
        if (existingComment == null || existingComment.UserId != User.GetSignedInUserId()) return Forbid();

        _svc.UpdateComment(comment);
        return RedirectToAction("Details", new { id = comment.PostId });
    }

    //Delete Comment 
    [HttpPost]
    [Authorize]
    public IActionResult DeleteComment(int id)
    {
        var comment = _svc.GetCommentById(id);
        if (comment == null || comment.UserId != User.GetSignedInUserId()) return Forbid();
        
        _svc.DeleteComment(id);
        return RedirectToAction("Details", new { id = comment.PostId});
    }
}
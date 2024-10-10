using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Security;
using FinalProject.Web.Models.User;

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

    public ActionResult Index(int page=1, int size=20, string order="id", string direction="asc")
    {
        var paged = _svc.GetPosts(page,size,order,direction);
        return View(paged);
    }

    [Authorize]
    public ActionResult Create()
    {
        return View();
    }

    //HTTP POST - Create a new Post
    [Authorize]
    [HttpPost]
    public ActionResult Create(Post post)
    {
        if (ModelState.IsValid)
        {
            post.UserId = User.GetSignedInUserId();
            _svc.AddPost(post);
            return RedirectToAction("Index");
        }
        return View(post);
    }
}
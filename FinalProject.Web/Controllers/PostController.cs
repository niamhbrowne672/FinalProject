using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Extensions;
using FinalProject.Web.Models.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/**
* Post Controller
*/
namespace FinalProject.Web.Controllers;

public class PostController : BaseController
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    //HTTP GET - Display Paged List of Posts

    public IActionResult Index(string searchQuery, int page=1, int size=20, string order="id", string direction="asc")
    {
        var query = string.IsNullOrWhiteSpace(searchQuery) 
            ? _postService.GetAllPosts()
            : _postService.SearchPosts(searchQuery);

        //apply pagination, sorting etc
        var pagedPosts = query.ToPaged(page, size, order);

        //pass the search query back to the view for display in the search bar
        ViewBag.SearchQuery = searchQuery;

        return View(pagedPosts);
    }

    public IActionResult Details(int id)
    {
        var postItem = _postService.GetPostById(id);

        if (postItem == null)
        {
            Alert($"Post {id} not found.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(postItem);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    //HTTP POST - Create a new Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Post p)
    {
        //validate title as unique
        if (_postService.GetPostByTitle(p.Title) != null)
        {
            ModelState.AddModelError(nameof(p.Title), "Post already exists.");
        }

        //complete POST action to add post
        if (ModelState.IsValid)
        {
            p = _postService.AddPost(p);
            if (p != null)
            {
                return RedirectToAction(nameof(Details), new { id = p.Id });
            }
        }
        return View(p);
    }

    [Authorize(Roles = "admin")]
    public IActionResult Edit(int id)
    {
        //load the post using the service
        var postItem = _postService.GetPostById(id);

        //check if post is null
        if (postItem == null)
        {
            Alert($"Post {id} not found.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        return View(postItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult Edit(int id, Post updatedPost)
    {
        //check if the provided even ID matches the ID of the updated post
        if (id != updatedPost.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingPost = _postService.GetPostById(id);

            //check if the post exists in the database
            if (existingPost == null)
            {
                Alert($"Post {id} not found.", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.PostedOn = updatedPost.PostedOn;
            existingPost.CreatedBy = updatedPost.CreatedBy;
            existingPost.ImagePath = updatedPost.ImagePath;

            //save the changes to the database
            var savedPost = _postService.UpdatePost(existingPost);

            if (savedPost != null)
            {
                Alert($"Post updated.", AlertType.success);
                return RedirectToAction(nameof(Details), new { id = savedPost.Id });
            }
        }
        //if ModelState is not valid or post update failed, redisplay the form for editing
        return View(updatedPost);
    }

    [Authorize(Roles = "admin")]
    public IActionResult Delete(int id)
    {
        //load the post using the service
        var postItem = _postService.GetPostById(id);
        //check the returned post is not null
        if (postItem == null)
        {
            Alert($"Post {id} could not be deleted.", AlertType.danger);
            return RedirectToAction(nameof(Index));
        }

        //pass post to view for deletion confirmation
        return View(postItem);
    }

    [HttpPost, ActionName("DeleteConfirm")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteConfirmed(int id)
    {
        //delete event via service
        var deleted = _postService.DeletePost(id);
        if (deleted)
        {
            Alert("Post Deleted.", AlertType.success);
        }
        else
        {
            Alert("Post could not be deleted", AlertType.warning);
        }

        //redirect to the index view
        return RedirectToAction(nameof(Index));
    }


    //========================= Post Comment Section ====================
    public IActionResult CommentCreate(int id)
    {
        var postItem = _postService.GetPostById(id);
        if (postItem == null)
        {
            Alert("Post does not exist.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        //create a comment view model and set foreign key
        var comment = new Comment { PostId = id };
        //render blank form
        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CommentCreate(Comment comment)
    {
        if (ModelState.IsValid)
        {                
            var createdComment = _postService.CreateComment(comment); 
            if (createdComment != null)
            {
                Alert("Comment Created Successfully.", AlertType.success);
                return RedirectToAction(nameof(Details), new { id = comment.PostId});
            }
            else
            {
                Alert("Comment could not be created.", AlertType.warning);
            }
        }
        // redisplay the form for editing
        return View(comment);
    }

    [Authorize(Roles = "admin")]
    public IActionResult CommentDelete(int id)
    {
        // load the comment using the service
        var comment = _postService.GetComment(id);
        // check the returned comment is not null
        if (comment == null)
        {
            Alert("Comment does not exist.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }     
        
        // pass comment to view for deletion confirmation
        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult CommentDeleteConfirm(int id, int postId)
    {
        var deleted = _postService.DeleteComment(id);

        if (deleted)
            {
                Alert("Comment deleted Successfully.", AlertType.success);
            }
            else
            {
                Alert("Comment could not be deleted.", AlertType.warning);
            }

        // redirect to the post details view
        return RedirectToAction(nameof(Details), new { Id = postId });
    }
}
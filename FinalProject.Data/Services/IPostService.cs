using FinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Extensions;
namespace FinalProject.Data.Services;

public interface IPostService
{
    //Get all posts
    Paged<Post> GetPosts();
    Paged<Post> GetPosts(int page=1, int pageSize=20, string orderBy = "id", string direction = "asc");
    Post GetPostByTitle(string title);
    
    //Add a new post
    Post AddPost(Post p);
    //Get a post by id
    Post GetPostById(int id);

    //Update a post 
    Post UpdatePost(Post updated);

    //Delete a post 
    bool DeletePost(int id);
    IQueryable<Post> GetAllPosts();

    IQueryable<Post> SearchPosts(string searchQuery);

    //------------------------------------------Comment Section--------------------------------
    Comment CreateComment(int id, string comment, string createdBy);
    Comment CreateComment(Comment comment);
    Comment GetComment(int id);
    //IEnumerable<Comment> GetCommentsByPostId(int postId);
    //Comment GetCommentById(int id);
    //Comment UpdateComment(Comment comment);
    bool DeleteComment(int id);
    IList<Comment> GetAllComments();
}
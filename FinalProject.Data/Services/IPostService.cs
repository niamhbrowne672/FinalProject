using FinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace FinalProject.Data.Services;

public interface IPostService
{
    //Get all posts
    Paged<Post> GetPosts();
    Paged<Post> GetPosts(int page, int pageSize, string orderBy = "id", string direction = "asc");

    //Add a new post
    Post AddPost(Post post);
    //Get a post by id
    Post GetPostById(int id);

    //Comment Section
    Comment AddComment(int postId, Comment comment);
    IEnumerable<Comment> GetCommentsByPostId(int postId);
    Comment GetCommentById(int id);
    Comment UpdateComment(Comment comment);
    void DeleteComment(int postId);



    //Update a post 
    Post UpdatePost(Post post);

    //Delete a post 
    void DeletePost(int id);
}
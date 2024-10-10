using FinalProject.Data.Entities;
namespace FinalProject.Data.Services;

public interface IPostService
{
    //Get all posts
    Paged<Post> GetPosts();
    Paged<Post> GetPosts(int page, int pageSize, string orderBy = "id", string direction = "asc");

    //Add a new post
    Post AddPost(Post post);

    //Get a post by id
    //Post GetPost(int id);

    //Update a post 
    //Post UpdatePost(Post post);

    //Delete a post 
    //void DeletePost(int id);
}
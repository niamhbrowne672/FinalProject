using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;
using FinalProject.Data.Extensions;

using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data.Services;

public class PostServiceDb : IPostService
{
    private readonly DatabaseContext ctx;

    public PostServiceDb(DatabaseContext ctx)
    {
        this.ctx = ctx;
    }

    public void Initialise()
    {
        ctx.Initialise();
    }

    public Paged<Post> GetPosts(int page, int pageSize, string orderBy = "id", string direction = "asc")
    {
        var query = (orderBy.ToLower(),direction.ToLower()) switch
        {
            ("id","asc")         => ctx.Posts.OrderBy(r => r.Id),
            ("id","desc")        => ctx.Posts.OrderByDescending(r => r.Id),
            ("title","asc")      => ctx.Posts.OrderBy(r => r.Title),
            ("title","desc")     => ctx.Posts.OrderByDescending(r => r.Title),
            ("postedOn", "asc") => ctx.Posts.OrderBy(r => r.PostedOn),
            ("postedOn", "desc") => ctx.Posts.OrderByDescending(r => r.PostedOn),
            ("createdBy","asc")  => ctx.Posts.OrderBy(r => r.CreatedBy),
            ("createdBy","desc") => ctx.Posts.OrderByDescending(r => r.CreatedBy),
            _                    => ctx.Posts.OrderBy(r => r.Id)
        };

        return query.Include(p => p.User).ToPaged(page,pageSize,orderBy,direction);
    }

    public IQueryable<Post> SearchPosts(string searchQuery)
    {
       return ctx.Posts.Where(p => p.Title.Contains(searchQuery)); 
    }

     public IQueryable<Post> GetAllPosts()
    {
        return ctx.Posts;
    }

    public Paged<Post> GetPosts()
    {
        return ctx.Posts.ToPaged();
    }

    public Post GetPostById(int id)
    {
        return ctx.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
    }

    public Post GetPostByTitle(string title)
    {
        return ctx.Posts
        .Include(p => p.Comments)
        .FirstOrDefault(p => p.Title == title);
    }

    public Post AddPost(Post p)
    {
        //check if post exists
        if (GetPostByTitle(p.Title) != null)
        {
            return null;
        }

        //create new post
        var newPost = new Post
        {
            Title = p.Title,
            Content = p.Content,
            PostedOn = p.PostedOn,
            ImagePath = p.ImagePath,
            CreatedBy = p.CreatedBy
        };
        ctx.Posts.Add(newPost);
        ctx.SaveChanges();
        return newPost;
    }

    //delete the post identified by Id returning true if deleted and false if not found
    public bool DeletePost(int id)
    {
        var postToDelete = GetPostById(id);
        if (postToDelete == null)
        {
            return false;
        }
        ctx.Posts.Remove(postToDelete);
        ctx.SaveChanges();
        return true;
    }

    //Update the post with the details in updated
    public Post UpdatePost(Post updated)
    {
        //verify the post exists
        var existingPost = GetPostById(updated.Id);
        if (existingPost == null)
        {
            return null;
        }

        //verify post is still unique
        var postWithSameTitle = GetPostByTitle(updated.Title);
        if (postWithSameTitle != null && postWithSameTitle.Id != updated.Id)
        {
            Console.WriteLine($"Post with title '{updated.Title}' already exists with a different ID.");
            return null;
        }

        //update the details of the post retrieved and save
        existingPost.Title = updated.Title;
        existingPost.Content = updated.Content;
        existingPost.PostedOn = updated.PostedOn;
        existingPost.CreatedBy = updated.CreatedBy;
        existingPost.ImagePath = updated.ImagePath;

        ctx.SaveChanges();
        Console.WriteLine($"Post with ID {updated.Id} updated successfully.");
        return existingPost;
    }



    //=================================== Comments Section ==========================================

    //Add a new comment to a post
    public Comment CreateComment(int postId, string comments, string createdBy)
    {
        var postToComment = GetPostById(postId);
        if (postToComment == null)
        { 
            return null;
        }
        var comment = new Comment
        {
            Comments = comments,
            CreatedBy = createdBy,
            CreatedAt = DateTime.Now
        };

        postToComment.Comments.Add(comment);
        ctx.SaveChanges();
        return comment;
    }

    public Comment CreateComment(Comment comment)
    {
        return CreateComment(comment.PostId, comment.Comments, comment.CreatedBy);
    }

    public Comment GetComment(int id)
    {
        return ctx.Comments.Include(c => c.Post).FirstOrDefault(c => c.Id == id);
    }

    public bool DeleteComment(int id)
    {
        //find review
        var commentToDelete = GetComment(id);
        if (commentToDelete == null)
        {
            return false;
        }

        //remove comment
        ctx.Comments.Remove(commentToDelete);

        ctx.SaveChanges();
        return true;
    }

    //retrieve all comments and the posts associated with the comment
    public IList<Comment> GetAllComments()
    {
        return ctx.Comments.Include(c => c.Post).ToList();
    }

    public Comment UpdateComment(int id, Comment updated)
    {
        var existingComment = GetComment(id);

        if (existingComment != null) return null;

        existingComment.Comments = updated.Comments;
        existingComment.CreatedBy = updated.CreatedBy;
        existingComment.CreatedAt = updated.CreatedAt;

        ctx.SaveChanges();
        return existingComment;
    }    
}
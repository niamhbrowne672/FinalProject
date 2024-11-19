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

    // public Post AddPost(Post post)
    // {
    //     ctx.Posts.Add(post);
    //     ctx.SaveChanges();
    //     return post;
    // }

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
            ("user.name","asc")  => ctx.Posts.OrderBy(r => r.User.Name),
            ("user.name","desc") => ctx.Posts.OrderByDescending(r => r.User.Name),
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
        return ctx.Posts.Include(p => p.User).Include(p => p.Comments).ThenInclude(c => c.User).FirstOrDefault(p => p.Id == id);
    }

    public Post GetPostByTitle(string title)
    {
        return ctx.Posts
        .Include(p => p.Comments)
        .FirstOrDefault(p => p.Title == title);
    }

    public Post AddPost(Post p)
    {
        //check if event exists
        if (GetPostByTitle(p.Title) != null)
        {
            return null;
        }

        //create new event
        var newPost = new Post
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            PostedOn = p.PostedOn,
            ImagePath = p.ImagePath
        };
        ctx.Posts.Add(newPost);
        ctx.SaveChanges();
        return newPost;
    }

    //delete the event identified by Id returning true if deleted and false if not found
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

    //Update the event with the details in updated
    public Post UpdatePost(Post updated)
    {
        //verify the event exists
        var existingPost = GetPostById(updated.Id);
        if (existingPost == null)
        {
            return null;
        }

        //verify event is still unique
        var postWithSameTitle = GetPostByTitle(updated.Title);
        if (postWithSameTitle != null && postWithSameTitle.Id != updated.Id)
        {
            Console.WriteLine($"Post with title '{updated.Title}' already exists with a different ID.");
            return null;
        }

        //update the details of the event retrieved and save
        existingPost.Title = updated.Title;
        existingPost.PostedOn = updated.PostedOn;
        existingPost.Content = updated.Content;
        existingPost.ImagePath = updated.ImagePath;

        ctx.SaveChanges();
        Console.WriteLine($"Post with ID {updated.Id} updated successfully.");
        return existingPost;
    }



    //=================================== Comments Section ==========================================

    //Add a new comment to a post
    public Comment CreateComment(int postId, string name, string comments)
    {
        var postToComment = GetPostById(postId);
        if (postToComment == null)
        { 
            return null;
        }
        var comment = new Comment
        {
            Name = name,
            Comments = comments,
            CreatedAt = DateTime.Now
        };

        postToComment.Comments.Add(comment);
        ctx.SaveChanges();
        return comment;
    }

    public Comment CreateComment(Comment comment)
    {
        return CreateComment(comment.PostId, comment.Name, comment.Comments);
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

        //remove review
        ctx.Comments.Remove(commentToDelete);

        ctx.SaveChanges();
        return true;
    }

    //retrieve all reviews and the events associated with the review
    public IList<Comment> GetAllComments()
    {
        return ctx.Comments.Include(c => c.Post).ToList();
    }

    public Comment UpdateComment(int id, Comment updated)
    {
        var existingComment = GetComment(id);

        if (existingComment != null) return null;

        existingComment.Name = updated.Name;
        existingComment.CreatedAt = updated.CreatedAt;
        existingComment.Comments = updated.Comments;

        ctx.SaveChanges();
        return existingComment;
    }


    
}
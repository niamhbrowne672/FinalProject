using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;
using FinalProject.Data.Extensions;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

    public Post AddPost(Post post)
    {
        ctx.Posts.Add(post);
        ctx.SaveChanges();
        return post;
    }

    public Paged<Post> GetPosts(int page, int pageSize, string orderBy = "id", string direction = "asc")
    {
        var query = (orderBy.ToLower(),direction.ToLower()) switch
        {
            ("id","asc")         => ctx.Posts.OrderBy(r => r.Id),
            ("id","desc")        => ctx.Posts.OrderByDescending(r => r.Id),
            ("title","asc")      => ctx.Posts.OrderBy(r => r.Title),
            ("title","desc")     => ctx.Posts.OrderByDescending(r => r.Title),
            ("user.name","asc")  => ctx.Posts.OrderBy(r => r.User.Name),
            ("user.name","desc") => ctx.Posts.OrderByDescending(r => r.User.Name),
            _                    => ctx.Posts.OrderBy(r => r.Id)
        };

        return query.Include(p => p.User).ToPaged(page,pageSize,orderBy,direction);
    }

    public Paged<Post> GetPosts()
    {
        return ctx.Posts.Include(p => p.User).ToPaged();
    }

    public Post GetPostById(int id)
    {
        return ctx.Posts.Include(p => p.User).Include(p => p.Comments).ThenInclude(c => c.User).FirstOrDefault(p => p.Id == id);
    }

    //Add a new comment to a post
    public Comment AddComment(int postId, Comment comment)
    {
        var post = ctx.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == postId);
        if (post == null) return null;

        //Attach the comment to the post
        comment.PostId = postId;
        ctx.Comments.Add(comment);
        ctx.SaveChanges();

        return comment;
    }

    //Get all comments for a specific post
    public IEnumerable<Comment> GetCommentsByPostId(int postId)
    {
        return ctx.Comments.Where(c => c.PostId == postId).Include(c => c.User).ToList();
    }

    //Get a specific comment by its ID
    public Comment GetCommentById(int id)
    {
        return ctx.Comments.Include(c => c.User).FirstOrDefault(c => c.Id == id);
    }

    //Update an existing comment
    public Comment UpdateComment(Comment comment)
    {
        var existingComment = ctx.Comments.Find(comment.Id);
        if (existingComment == null) return null;

        existingComment.Content = comment.Content;
        existingComment.ModifiedAt = comment.ModifiedAt;
        ctx.SaveChanges();

        return existingComment;
    }

    public void DeleteComment(int id)
    {
        var comment = ctx.Comments.Find(id);
        if (comment == null) return;

        ctx.Comments.Remove(comment);
        ctx.SaveChanges();
    }

    //UpdatePost
    public Post UpdatePost(Post post)
    {
        var existingPost = ctx.Posts.Find(post.Id);
        if (existingPost == null) return null;

        existingPost.Title = post.Title;
        existingPost.Content = post.Content;
        existingPost.ModifiedAt = post.ModifiedAt;
        ctx.SaveChanges();

        return existingPost;
    }

    //DeletePost
    public void DeletePost(int id)
    {
        var post = ctx.Posts.Find(id);
        if(post == null) return;

        ctx.Posts.Remove(post);
        ctx.SaveChanges();
    }
}
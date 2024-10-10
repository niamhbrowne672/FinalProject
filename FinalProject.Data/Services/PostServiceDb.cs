using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;

using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data.Services;

public class PostServiceDb : IPostService
{
    private readonly DatabaseContext ctx;

    public PostServiceDb(DatabaseContext ctx)
    {
        this.ctx = ctx;
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

    public void Initialise()
    {
        ctx.Initialise();
    }
}
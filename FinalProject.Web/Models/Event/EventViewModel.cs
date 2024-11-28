using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Identity.Client;

namespace FinalProject.Web.Models.Event;
public class EventViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime EventTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Likes { get; set; }
    public bool UserHasLiked { get; set; }
}
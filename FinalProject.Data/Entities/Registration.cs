using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus.Extensions.UnitedKingdom;

public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime RegisteredOn { get; set; }
}
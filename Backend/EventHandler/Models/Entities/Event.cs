using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHandler.Models.Entities;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string OrganizerId { get; set; }

    public string EventName { get; set; }

    public string Slug { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Location { get; set; }

    public string EventType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CategoryId { get; set; }

    public Category category { get; set; }

    public AppUser Organizer { get; set; }


    public List<Ticket> tickets { get; set; }

}

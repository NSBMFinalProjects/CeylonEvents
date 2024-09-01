using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Event")]
    public int EventId { get; set; }

    public string Message { get; set; }

    public DateTime SentAt { get; set; }

    public bool ReadStatus { get; set; }

    public User User { get; set; }

    public Event Event { get; set; }
}

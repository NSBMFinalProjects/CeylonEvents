using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHandler.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    public int PhoneNo { get; set; }

    public List<UserUserRoles> UserRoles { get; set; }

    public List<EventAttendance> Attendances { get; set; }

    public List<Notification> notifications { get; set; }

    public List<Ticket> tickets { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

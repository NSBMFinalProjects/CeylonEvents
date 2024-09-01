using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public List<UserRole> Roles { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

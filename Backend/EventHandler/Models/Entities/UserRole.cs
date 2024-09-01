using System.ComponentModel.DataAnnotations;

public class UserRole
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string RoleName { get; set; }

    public string Description { get; set; }

    public List<User> Users { get; set; }
}

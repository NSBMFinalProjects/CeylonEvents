namespace EventHandler.Models.Entities
{
    public class UserUserRoles
    {
        public int UserId { get; set; } // Foreign key to User

        public User User { get; set; } // Navigation property

        public int UserRoleId { get; set; } // Foreign key to UserRole

        public UserRole UserRole { get; set; } // Navigation property
    }

}

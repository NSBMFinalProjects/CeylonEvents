using System;
using System.ComponentModel.DataAnnotations;

namespace frontend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "First name is required!")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;  

        [Required(ErrorMessage = "Enter a valid NIC number")]
        [StringLength(12)] 
        public string Nic { get; set; } = string.Empty; 
        
        [Required(ErrorMessage = "Enter a valid email address")]
        [StringLength(50)]  
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a valid password")]
        [StringLength(20)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter a valid contact number")]
        [StringLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;  

        [Required(ErrorMessage = "Re enter your password")]
        [StringLength(20)]
        public string ConfirmePassword { get; set; } = string.Empty;
    }
}



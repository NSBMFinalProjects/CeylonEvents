﻿using System.ComponentModel.DataAnnotations;

namespace EventHandler.Dto
{
    public record class RegisterDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string NIC {  get; set; } =string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; }= string.Empty;

        [Required]
        public string Password {  get; set; }= string.Empty;

        [Required]
        public string PhoneNumber { get; set; }=string.Empty;
    }   
}

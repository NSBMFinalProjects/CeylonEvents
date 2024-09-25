namespace EventHandler.Dto
{
    public record class UserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string NIC { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}

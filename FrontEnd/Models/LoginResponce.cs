using System;

namespace frontend.Models;

public class LoginResponce
{
    public string token { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}

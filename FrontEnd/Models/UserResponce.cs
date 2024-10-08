using System;
using System.Text.Json.Serialization;

namespace frontend.Models;

public class UserResponce
{

    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("events")]
    public List<EventResponce>? Events { get; set; }

}

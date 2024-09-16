﻿using Microsoft.AspNetCore.Identity;

namespace EventHandler.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }

        public int reqId {  get; set; }

        public List<Event> Events { get; set; }

        public List<Ticket> Tickets { get; set; }

        public Requests Requests {  get; set; }
    }
}
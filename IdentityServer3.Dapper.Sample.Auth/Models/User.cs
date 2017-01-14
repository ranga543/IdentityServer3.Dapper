using System;

namespace IdentityServer3.Dapper.Sample.Auth.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
    }
}
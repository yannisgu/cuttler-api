using System;

namespace Cuttler.Entities
{
    public class Login
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
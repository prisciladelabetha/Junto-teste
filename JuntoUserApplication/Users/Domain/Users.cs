using System;

namespace Junto.Users.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User(Guid id, string username, string password)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
        }
    }
}
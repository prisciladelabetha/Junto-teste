using System;

namespace Junto.Users.Domain
{
    public class User
    {
        public Guid Id { get; }

        public string Username { get; }

        public string Password { get; }

        public User(Guid id, string username, string password)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
        } 
    }
}
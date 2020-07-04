using System;

namespace Junto.Users.Domain
{
    public class AuthenticatedUser
    {
        public Guid Id { get; }
        public string Username { get; }
        public string Token { get; }

        public AuthenticatedUser(Guid id, string username, string token)
        {
            this.Id = id;
            this.Username = username;
            this.Token = token;
        }
    }

}